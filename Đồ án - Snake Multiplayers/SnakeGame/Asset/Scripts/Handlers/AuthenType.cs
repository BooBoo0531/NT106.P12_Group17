using Assets.Scripts;
using Assets.Scripts.Handlers;
using Assets.Scripts.Handlers.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.VersionControl;

namespace Snake_Royale_Server.Handlers
{
    public class AuthenType : TransferType
    {
        public AuthenType(Client client) : base(client)
        {
        }

        public AuthenType(Client client, int typeNum) : base(client, typeNum)
        {
        }

        public enum Type
        {
            Register = 2,
            Logout = 3,
            StartGame = 4,
        }

        public override string GetType()
        {
            switch (TypeNum)
            {
                case (int)Type.Register:
                    return Type.Register.ToString();
                case (int)Type.Logout:
                    return Type.Logout.ToString();
                    case (int)Type.StartGame:
                        return Type.StartGame.ToString();
                default:
                    return DEFAULT_TYPE;
            }
        }

        public override void HandleAction(string actionData)
        {
            switch (TypeNum)
            {
                case (int)Type.Register:
                    Register();
                    break;
                case (int)Type.Logout:
                    Logout();
                    break;
                    case (int)Type.StartGame:
                    StartGame(actionData);
                        break;
                default:
                    break;
            }
        }

        private void StartGame(string actionData)
        {
            string snakeJson = actionData.Split(";")[1];
            var nSnake = JsonConvert.DeserializeObject<SnakeDTO>(snakeJson);
            GameHandler.Instance.NewSnakeJoin(nSnake);
        }

        public void Register()
        {

        }

        public void Logout()
        {

        }
    }
}
