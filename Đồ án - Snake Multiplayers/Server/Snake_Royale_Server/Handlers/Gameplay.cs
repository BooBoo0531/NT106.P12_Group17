using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snake_Royale_Server.Handlers
{
    public class Gameplay
    {
        private volatile static Gameplay instance;

        public static Gameplay Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Gameplay();
                }
                return instance;
            }
        }
        public Dictionary<TcpClient, ServerThread> ServerThreads { get; set; } = new Dictionary<TcpClient, ServerThread>();

        public LevelGridDTO LevelGrid { get; set; }

        public Gameplay()
        {
        }

        public void ResetGame()
        {
            LevelGrid = null;
            ServerThreads.Clear();
        }

        public bool RemoveServerThread(TcpClient client)
        {
            if (ServerThreads.Count == 0)
            {
                return false;
            }
            if (!ServerThreads.Keys.Contains(client))
            {
                return false;
            }
            ServerThreads.Remove(client);
            return true;
        }

        public bool RemoveServerThread(ServerThread serverThread)
        {
            if(ServerThreads.Count == 0)
            {
                return false;
            }
            if(!ServerThreads.Values.Contains(serverThread))
            {
                return false;
            }
            serverThread.Stop();
            ServerThreads.Remove(serverThread.Client);
            return true;
        }

        public void BroadcastMessage(string message)
        {
            foreach (var serverThread in ServerThreads.Values)
            {
                if(!serverThread.IsPlaying)
                {
                    continue;
                }
                serverThread.sendMessageToClient(message);
            }
        }

        public void BroadcastMessageExcept(string message, ServerThread exceptThread)
        {
            foreach (var serverThread in ServerThreads.Values)
            {
                if (!serverThread.IsPlaying)
                {
                    continue;
                }
                if (exceptThread == serverThread)
                {
                    continue;
                }
                serverThread.sendMessageToClient(message);
            }
        }

        public void AddServerThread(TcpClient client, ServerThread serverThread)
        {
            ServerThreads.Add(client, serverThread);
        }

        public string GetLevelGrid()
        {
            return JsonSerializer.Serialize(LevelGrid);
        }
    }
}
