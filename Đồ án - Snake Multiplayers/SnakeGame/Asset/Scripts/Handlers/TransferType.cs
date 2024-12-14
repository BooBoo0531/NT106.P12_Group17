using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public Client Client { get; set; }
        public int TypeNum { get; set; }

        public TransferType(Client client)
        {
            this.Client = client;
        }

        public TransferType(Client client, int typeNum)
        {
            Client = client;
            TypeNum = typeNum;
        }

        public abstract string GetType();

        public abstract void HandleAction(string actionData);
    }
}
