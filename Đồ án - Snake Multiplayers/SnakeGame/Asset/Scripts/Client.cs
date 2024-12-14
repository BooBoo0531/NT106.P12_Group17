using Assets.Scripts.Handlers;
using Assets.Scripts.Handlers.DTO;
using Snake_Royale_Server.Handlers;
using System.Net.Sockets;
using UnityEngine;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System;
using System.Collections;
using Newtonsoft.Json;
using System.Linq;

namespace Assets.Scripts
{
    public class Client : MonoBehaviour
    {
        public static Client Instance { get { return instance; } }

        private static Client instance;
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        public TcpClient client;
        public bool connected = false;
        public static readonly string characters = ".-#";
        string server = "127.0.0.1";
        int port = 13000;
        NetworkStream Stream { get; set; }
        public Snake Snake { get; set; }

        [SerializeField] private GameObject loginForm;
        private async void Start()
        {
            instance = this;
            Snake = transform.GetComponent<Snake>();
            try
            {
                Debug.Log("Attempting to connect...");
                client = new TcpClient();
                await client.ConnectAsync(server, port); // Asynchronous connection
                Debug.Log("Connected to the server");
                connected = true;
            }
            catch (SocketException ex)
            {
                Debug.LogError($"SocketException: {ex.Message}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception: {ex.Message}");
            }
        }

        private IEnumerator ListenToStreamCoroutine()
        {
            if (client != null && connected)
            {
                Byte[] lengthBytes = new Byte[4];
                Byte[] messageBytes;
                if (Stream == null)
                {
                    Stream = client.GetStream();
                }

                MoveType moveType = new MoveType(this, Snake);
                AuthenType authenType = new AuthenType(this);
                ActionType actionType = new ActionType(this, Snake);
                while (client != null && connected && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Task<int> bytesReadTask = Stream.ReadAsync(lengthBytes, 0, lengthBytes.Length);
                    yield return new WaitUntil(() => bytesReadTask.IsCompleted);
                    if (bytesReadTask.Result == 0)
                    {
                        continue;
                    }
                    int messageLength = BitConverter.ToInt32(lengthBytes, 0);
                    messageBytes = new Byte[messageLength];
                    int totalBytesRead = 0;
                    while (totalBytesRead < messageLength)
                    {
                        Task<int> totalBytesReadTask = Stream.ReadAsync(
                                                        messageBytes, totalBytesRead, messageLength - totalBytesRead);
                        yield return new WaitUntil(() => totalBytesReadTask.IsCompleted);
                        totalBytesRead += totalBytesReadTask.Result;
                    }
                    try
                    {
                        if (totalBytesRead > 0)
                        {
                            string responseData = Encoding.UTF8.GetString(messageBytes);
                            if (responseData.Contains(characters))
                            {
                                int type = int.Parse(responseData.Split(characters)[0]);
                                string message = responseData.Split(characters)[1];
                                int actionTypeNum = int.Parse(message.Split(";")[0]);
                                string actionData = message;
                                switch (type)
                                {
                                    case (int)TransferType.Type.MOVE:
                                        moveType.TypeNum = actionTypeNum;
                                        moveType.HandleAction(actionData);
                                        break;
                                    case (int)TransferType.Type.ACTION:
                                        actionType.TypeNum = actionTypeNum;
                                        actionType.HandleAction(actionData);
                                        break;
                                    case (int)TransferType.Type.AUTHEN:
                                        authenType.TypeNum = actionTypeNum;
                                        authenType.HandleAction(actionData);
                                        break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.Log($"Error in coroutine read {ex.Message}");
                        Destroy(gameObject);
                    }
                }
                Destroy(gameObject);
                yield return new WaitForSeconds(0.1f); // Prevent tight loop
            }
        }

        private void OnDestroy()
        {
            if (Snake != null && !string.IsNullOrEmpty(Snake.Username))
            {
                sendData($"{(int)TransferType.Type.ACTION}{characters}{(int)ActionType.Type.DIE};{Snake.Username}");
            }
            if (client != null)
            {
                client.Close();
            }
        }

        public void ConnectServer(string username)
        {
            try
            {
                Stream = client.GetStream();
                GameHandler.Instance.StartGame();
                var levelGrid = GameHandler.Instance.LevelGrid;
                var posLevel = levelGrid.GetPos();
                var size = levelGrid.GetSize();
                string loginMessage = $"{(int)TransferType.Type.AUTHEN}{characters}{(int)AuthenType.Type.StartGame};{username};{posLevel.x};{posLevel.y};{size.x};{size.y}";
                sendData(loginMessage);
                Byte[] lengthBytes = new Byte[4];
                Byte[] messageBytes;
                int bytesRead = Stream.Read(lengthBytes, 0, lengthBytes.Length);
                if (bytesRead == 0)
                {
                    return;
                }
                int messageLength = BitConverter.ToInt32(lengthBytes, 0);
                messageBytes = new Byte[messageLength];
                int totalBytesRead = 0;
                while (totalBytesRead < messageLength)
                {
                    totalBytesRead += Stream.Read(
                        messageBytes, totalBytesRead, messageLength - totalBytesRead);
                }
                if (totalBytesRead == 0)
                {
                    return;
                }
                string responseData = Encoding.UTF8.GetString(messageBytes);
                int type = int.Parse(responseData.Split(characters)[0]);
                string message = responseData.Split(characters)[1];
                int actionTypeNum = int.Parse(message.Split(";")[0]);
                string nUsername = message.Split(";")[1];
                string actionData = message.Split(";")[2];
                if (actionTypeNum == (int)AuthenType.Type.StartGame)
                {
                    if (nUsername != Snake.Username)
                    {
                        Snake.Username = nUsername;
                        GameCacheData.Instance.retryUsername = nUsername;
                    }
                    var levelGridDTO = JsonConvert.DeserializeObject<LevelGridDTO>(actionData);
                    GameHandler.Instance.SetLevelGrid(levelGridDTO, Snake);
                    StartCoroutine(ListenToStreamCoroutine());
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Tài khoản này đang được sử dụng!");
                Destroy(gameObject);
            }
        }

        public void sendData(string data)
        {
            try
            {
                if (Stream == null)
                {
                    Debug.Log($"Not starting process error send: {data}");
                    return;
                }
                byte[] lengthBytes = BitConverter.GetBytes(data.Length);
                byte[] ms = Encoding.UTF8.GetBytes(data);
                Stream.Write(lengthBytes, 0, lengthBytes.Length);
                Stream.Write(ms, 0, ms.Length);
            }
            catch (Exception ex)
            {
                Debug.Log($"Error send: {ex.ToString()}");
            }
        }


        public void Login(string username)
        {
            if (Snake == null || client == null || !connected)
            {
                return;
            }
            Snake.Username = username;
            GameCacheData.Instance.retryUsername = username;
            ConnectServer(username);
        }
    }
}
