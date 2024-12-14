using Snake_Royale_Server.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Snake_Royale_Server.Handlers
{
    public abstract class TransferType
    {
        public static readonly string DEFAULT_TYPE = "None";
        public enum Type
        {
            MOVE = 1,
            ACTION = 2,
            AUTHEN = 3,
        }
        public ServerThread ServerThread { get; set; }
        
        public NetworkStream Stream { get; set; }
        public int TypeNum { get; set; }

        public TransferType(ServerThread serverThread)
        {
            ServerThread = serverThread;
        }

        public TransferType(ServerThread serverThread, int typeNum)
        {
            ServerThread = serverThread;
            TypeNum = typeNum;
        }

        public abstract string GetType();

        public abstract void HandleAction(string actionData);
    }
}
