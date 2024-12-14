using Snake_Royale_Server.Models;
using Snake_Royale_Server.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Snake_Royale_Server.Handlers
{
    public class AuthenType : TransferType
    {
        public AuthenType(ServerThread serverThread) : base(serverThread)
        {
        }

        public AuthenType(ServerThread serverThread, int typeNum) : base(serverThread, typeNum)
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
            string username = actionData.Split(";")[1];
            float posX = float.Parse(actionData.Split(";")[2]);
            float posY = float.Parse(actionData.Split(";")[3]);
            float width = float.Parse(actionData.Split(";")[4]);
            float height = float.Parse(actionData.Split(";")[5]);
            if (Gameplay.Instance.LevelGrid == null)
            {
                Gameplay.Instance.LevelGrid = new LevelGridDTO(posX, posY, width, height);
            }
            var snake = new SnakeDTO();
            snake.Username = username;
            Gameplay.Instance.LevelGrid.Setup(snake);
            ServerThread.sendMessageToClient($"{(int)TransferType.Type.AUTHEN}{ServerThread.characters}{(int)Type.StartGame};{snake.Username};{Gameplay.Instance.GetLevelGrid()}");
            string snakeJson = JsonSerializer.Serialize(snake);
            Gameplay.Instance.BroadcastMessageExcept($"{(int)TransferType.Type.AUTHEN}{ServerThread.characters}{(int)Type.StartGame};{snakeJson}", ServerThread);
            ServerThread.Username = snake.Username;
            var existAccount = ServerThread._accountRepo.Get(snake.Username);
            if (existAccount == null)
            {
                var nAccount = new Account
                {
                    Username = snake.Username,
                    MaxScore = 0,
                    GamePlayed = 1,
                };
                ServerThread._accountRepo.Add(nAccount);
                ServerThread.Account = nAccount;
            }
            else
            {
                ServerThread.Account = existAccount;
                ServerThread.Account.GamePlayed++;
                ServerThread._accountRepo.Update(ServerThread.Account);
            }
            if (ServerThread.Account == null)
            {
                ServerThread.AddText(ServerThread.Username + " Login Error!");
            }
            else
            {
                ServerThread.AddText(ServerThread.Username + " Login");
            }
            ServerThread.IsPlaying = true;
        }

        public void Register()
        {

        }

        public void Logout()
        {

        }
    }
}
