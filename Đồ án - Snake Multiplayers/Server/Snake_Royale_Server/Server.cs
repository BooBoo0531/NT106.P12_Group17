using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using Snake_Royale_Server.Handlers;

namespace Snake_Royale_Server
{
    public class Server
    {
        public ServerForm ServerForm { get; set; }
        private TcpListener server = null;
        private Task currentTask = null;
        private CancellationTokenSource _stoppingSrc = new CancellationTokenSource();
        private Gameplay gameplay;
        public Server(ServerForm serverForm)
        {
            this.ServerForm = serverForm;
            gameplay = Gameplay.Instance;
        }

        public async void ExecuteServer(string host, int port)
        {
            try
            {
                 //= "Server Application";
                IPAddress localAddr = IPAddress.Parse(host);
                server = new TcpListener(port);
                server.Start();
                ServerForm.AddServerInfo($"{new string('*', 40)}");
                ServerForm.AddServerInfo($"Waiting for connection...");
                currentTask = new Task(() =>
                {
                    try
                    {
                        while (!_stoppingSrc.Token.IsCancellationRequested)
                        {
                            TcpClient client = server.AcceptTcpClient();
                            ServerThread serverThread = new ServerThread(this, client);
                            serverThread.Start();
                            ServerForm.Invoke(() =>
                            {
                                ServerForm.AddServerInfo($"New connection join game.");
                            });
                            gameplay.AddServerThread(client, serverThread);
                        }
                    } catch(Exception ex)
                    {

                    }
                });
                currentTask.Start();
                
            }
            catch (Exception ex)
            {
                ServerForm.AddServerInfo($"Exception: {ex.Message}");
                StopServer();
            }
        }

        public void StopServer()
        {
            if(server != null)
            {
                server.Stop();
                server = null;
                ServerForm.AddServerInfo("Server stopped. Press any key to exit !");
                if (gameplay.ServerThreads != null && gameplay.ServerThreads.Count > 0)
                {
                    foreach (var serverThread in gameplay.ServerThreads.Values)
                    {
                        serverThread.Stop();
                    }
                }
                Gameplay.Instance.ResetGame();
                if (currentTask != null && currentTask.Status == TaskStatus.Running)
                {
                    try
                    {
                        _stoppingSrc.Cancel();
                    }
                    catch (Exception ex)
                    {

                    }
                    currentTask.Dispose();
                    currentTask = null;
                }
            }
        }
    }
}
