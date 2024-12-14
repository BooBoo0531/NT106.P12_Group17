using Snake_Royale_Server.Handlers;
using Snake_Royale_Server.Models;
using Snake_Royale_Server.Repository;
using System.Net.Sockets;
using System.Text;

namespace Snake_Royale_Server
{
    public class ServerThread
    {
        public static readonly string characters = ".-#";
        public TcpClient Client { get; set; }

        public Task Task { get; set; }

        public Server Server { get; set; }

        public NetworkStream Stream { get; set; }

        public string Username { get; set; }

        public Account Account { get; set; }

        public bool IsPlaying { get; set; } = false;

        public AccountRepository _accountRepo { get; set; } = new AccountRepository();
        public ServerThread(Server server, TcpClient client)
        {
            Client = client;
            Server = server;
        }

        public void Start()
        {
            Task = new Task(ProcessMessage);
            Task.Start();
        }

        public void Stop() {
            if (Task == null)
            {
                return;
            }
            try
            {
                if(Client != null && Client.Connected)
                {
                    Client.Close();
                }
                if(Task.IsCompleted == false)
                {
                    Task.Dispose();
                }
                Task = null;
            }
            catch(Exception ex )
            {
                Server.ServerForm.AddServerInfo($"Exception: {ex.Message}");
            }
        }

        private void ProcessMessage()
        {
            string data;
            int count;
            int type;
            string username;
            MoveType moveType = new MoveType(this);
            AuthenType authenType = new AuthenType(this);
            ActionType actionType = new ActionType(this);
            try
            {
                Byte[] lengthBytes = new Byte[4];
                byte[] messageBytes;
                Stream = Client.GetStream();
                while (true)
                {
                    int bytesRead = Stream.Read(lengthBytes, 0, lengthBytes.Length);
                    if (bytesRead == 0) continue;
                    int messageLength = BitConverter.ToInt32(lengthBytes, 0);
                    messageBytes = new byte[messageLength];
                    int totalBytesRead = 0;
                    while (totalBytesRead < messageLength)
                    {
                        totalBytesRead += Stream.Read(
                            messageBytes, totalBytesRead, messageLength - totalBytesRead);
                    }
                    if (totalBytesRead == 0) continue;
                    data = Encoding.UTF8.GetString(messageBytes);
                    string typeInData = data.Split(characters)[0];
                    string msgInData = data.Split(characters)[1];
                    type = int.Parse(typeInData);
                    int actionTypeNum = int.Parse(msgInData.Split(";")[0]);
                    string actionData = msgInData;
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
            catch (Exception ex)
            {
                Server.ServerForm.AddServerInfo($"Exception: {ex.Message}");
                if (!string.IsNullOrEmpty(Username))
                {
                    actionType.Die($"{(int)ActionType.Type.DIE};{Username};");
                } else
                {
                    Gameplay.Instance.RemoveServerThread(this);
                }
                if(Client != null && Client.Connected)
                {
                    Client.Close();
                }
                Task = null;
            }
        }

        public void sendMessageToClient(string data)
        {
            try
            {
                if (Stream == null)
                {
                    Server.ServerForm.AddServerInfo($"Not starting process error send: {data}");
                    return;
                }
                byte[] lengthBytes = BitConverter.GetBytes(data.Length);
                byte[] ms = Encoding.UTF8.GetBytes(data);
                Stream.Write(lengthBytes, 0, lengthBytes.Length);
                Stream.Write(ms, 0, ms.Length);
            } catch(Exception ex)
            {
                Server.ServerForm.AddServerInfo($"Server Send Error: {ex.ToString()}");
            }
        }

        public ServerForm GetServerForm()
        {
            if (Server == null)
            {
                return null;
            }
            return Server.ServerForm;
        }

        public void AddText(string text)
        {
            Server.ServerForm.AddServerInfo(text);
        }
    }
}
