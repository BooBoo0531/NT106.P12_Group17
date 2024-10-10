using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Server : Form
    {
        private Thread listenThread;
        private TcpListener tcpListener;
        private bool stopChatServer = true;
        private readonly int _serverPort = 8000;
        private Dictionary<string, TcpClient> dict = new Dictionary<string, TcpClient>();
        private Thread replyThread;
        public Server()
        {
            InitializeComponent();
        }

        public void Listen()
        {
            try
            {
                tcpListener = new TcpListener(new IPEndPoint(IPAddress.Parse(textBox1.Text), _serverPort));
                tcpListener.Start();

                while (!stopChatServer)
                {
                    TcpClient _client = tcpListener.AcceptTcpClient();

                    StreamReader sr = new StreamReader(_client.GetStream());
                    StreamWriter sw = new StreamWriter(_client.GetStream());
                    sw.AutoFlush = true;
                    string username = sr.ReadLine();
                    if (string.IsNullOrEmpty(username))
                    {
                        sw.WriteLine("Please pick a username");
                        _client.Close();
                    }
                    else
                    {
                        if (!dict.ContainsKey(username))
                        {
                            Thread clientThread = new Thread(() => this.ClientRecv(username, _client));
                            dict.Add(username, _client);
                            clientThread.Start();
                        }
                        else
                        {
                            sw.WriteLine("Username already exist, pick another one");
                            _client.Close();
                        }
                    }

                }
            }
            catch (SocketException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ClientRecv(string username, TcpClient tcpClient)
        {
            StreamReader sr = new StreamReader(tcpClient.GetStream());
            try
            {
                while (!stopChatServer)
                {
                    Application.DoEvents();
                    string userNameAndMsg = sr.ReadLine();
                    string[] arrPayload = userNameAndMsg.Split(';');
                    string friendUsername = arrPayload[0];
                    string msg = arrPayload[1];
                    string formattedMsg = $"[{DateTime.Now:MM/dd/yyyy h:mm tt}] {username}: {msg}\n";
                    TcpClient friendTcpClient;

                    if (msg.StartsWith("File: ")) 
                    {
                        string[] fileParts = msg.Split(';');
                        string fileName = fileParts[0].Replace("File: ", "");
                        byte[] fileData = Convert.FromBase64String(fileParts[1]); 

                        File.WriteAllBytes($"received_{fileName}", fileData); 
                        UpdateChatHistoryThreadSafe($"{username} sent a file: {fileName}\n");

                        if (dict.TryGetValue(friendUsername, out friendTcpClient))
                        {
                            StreamWriter sw = new StreamWriter(friendTcpClient.GetStream());
                            sw.WriteLine($"[{DateTime.Now:MM/dd/yyyy h:mm tt}] {username} sent a file: {fileName}\n");
                            sw.AutoFlush = true;
                        }
                    }
                    else 
                    {
                        if (dict.TryGetValue(friendUsername, out friendTcpClient))
                        {
                            StreamWriter sw = new StreamWriter(friendTcpClient.GetStream());
                            sw.WriteLine(formattedMsg);
                            sw.AutoFlush = true;
                        }

                        StreamWriter sw2 = new StreamWriter(tcpClient.GetStream());
                        sw2.WriteLine(formattedMsg);
                        sw2.AutoFlush = true;

                        UpdateChatHistoryThreadSafe(formattedMsg);
                    }
                }
            }
            catch (SocketException sockEx)
            {
                tcpClient.Close();
                sr.Close();
            }
        }

        private delegate void SafeCallDelegate(string text);

        private void UpdateChatHistoryThreadSafe(string text)
        {
            if (richTextBox1.InvokeRequired)
            {
                var d = new SafeCallDelegate(UpdateChatHistoryThreadSafe);
                richTextBox1.Invoke(d, new object[] { text });
            }
            else
            {
                richTextBox1.Text += text;
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (stopChatServer)
            {
                stopChatServer = false;
                listenThread = new Thread(this.Listen);
                listenThread.Start();
                MessageBox.Show(@"Start listening for incoming connections");
                btnListen.Text = @"Stop";
            }
            else
            {
                stopChatServer = true;
                btnListen.Text = @"Start listening";
                tcpListener.Stop();
                listenThread = null;

            }
        }
    }
}
