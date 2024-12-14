using Snake_Royale_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Snake_Royale_Server.Handlers
{
    public class ActionType : TransferType
    {
        public ActionType(ServerThread serverThread) : base(serverThread)
        {
        }

        public ActionType(ServerThread serverThread, int typeNum) : base(serverThread, typeNum)
        {
        }

        public enum Type
        {
            EAT = 1,
            KILL = 2,
            DIE = 3,
            SPAWN_FOOD = 4,
            ADD_SCORE = 5,
            LOAD_RANKING = 6
        }

        public override string GetType()
        {
            switch (TypeNum)
            {
                case (int)Type.EAT:
                    return Type.EAT.ToString();
                case (int)Type.KILL:
                    return Type.KILL.ToString();
                case (int)Type.DIE:
                    return Type.DIE.ToString();
                case (int)Type.SPAWN_FOOD:
                    return Type.SPAWN_FOOD.ToString();
                case (int)Type.ADD_SCORE:
                    return Type.ADD_SCORE.ToString();
                case (int)Type.LOAD_RANKING:
                    return Type.LOAD_RANKING.ToString();
                default:
                    return DEFAULT_TYPE;
            }
        }

        public override void HandleAction(string actionData)
        {
            switch (TypeNum)
            {
                case (int)Type.EAT:
                    Eat(actionData);
                    break;
                case (int)Type.KILL:
                    Kill();
                    break;
                case (int)Type.DIE:
                    Die(actionData);
                    break;
                case (int)Type.SPAWN_FOOD:
                    SpawnFood();
                    break;
                case (int)Type.ADD_SCORE:
                    AddScore(actionData);
                    break;
                case (int)Type.LOAD_RANKING:
                    LoadRanking(actionData);
                    break;
                default:
                    break;
            }
        }

        private void LoadRanking(string actionData)
        {
            var accounts = ServerThread._accountRepo.GetAll();
            if (accounts == null || accounts.Count == 0)
            {
                accounts = new List<Account>();
            }
            else
            {
                accounts = accounts.OrderByDescending(x => x.MaxScore).ToList();
            }
            string accountsJson = JsonSerializer.Serialize(accounts);
            ServerThread.sendMessageToClient($"{(int)TransferType.Type.ACTION}{ServerThread.characters}{(int)Type.LOAD_RANKING};{accountsJson}");
        }

        private void AddScore(string actionData)
        {
            string username = actionData.Split(";")[1];
            long score = long.Parse(actionData.Split(";")[2]);
            //ServerThread.AddText("Add Score: " + username + " " + score);
            Gameplay.Instance.LevelGrid.ChangeSnakeScore(username, score);
            Gameplay.Instance.BroadcastMessageExcept($"{(int)TransferType.Type.ACTION}{ServerThread.characters}{(int)Type.ADD_SCORE};{username};{score}", ServerThread);
            if(ServerThread.Account == null)
            {
                ServerThread.Account = ServerThread._accountRepo.Get(username);
            }
            if (ServerThread.Account != null)
            {
                var snake = Gameplay.Instance.LevelGrid.GetSnake(username);
                long maxScore = 0;
                if (snake != null)
                {
                    maxScore = snake.Score;
                }
                if (maxScore > ServerThread.Account.MaxScore)
                {
                    ServerThread.Account.MaxScore = maxScore;
                    ServerThread._accountRepo.Update(ServerThread.Account);
                }
            }
        }

        public void Eat(string actionData)
        {
            string username = actionData.Split(";")[1];
            int bodySize = int.Parse(actionData.Split(";")[2]);
            Gameplay.Instance.LevelGrid.ChangeSnakeBodySize(username, bodySize);
            //ServerThread.AddText("Add Body Size: " + username + " " + bodySize);
            Gameplay.Instance.BroadcastMessageExcept($"{(int)TransferType.Type.ACTION}{ServerThread.characters}{(int)Type.EAT};{username};{bodySize}", ServerThread);
        }

        public void Kill()
        {
            throw new NotImplementedException();
        }

        public void Die(string actionData)
        {
            string username = actionData.Split(";")[1];
            var snake = Gameplay.Instance.LevelGrid.GetSnake(username);
            long maxScore = 0;
            if (snake != null)
            {
                maxScore = snake.Score;
            }
            Gameplay.Instance.LevelGrid.RemoveSnake(username);
            bool res = Gameplay.Instance.RemoveServerThread(ServerThread);
            if (res)
            {
                Gameplay.Instance.BroadcastMessage($"{(int)TransferType.Type.ACTION}{ServerThread.characters}{(int)Type.DIE};{username};");
                if (ServerThread.Account != null)
                {
                    if (maxScore > ServerThread.Account.MaxScore)
                    {
                        ServerThread.Account.MaxScore = maxScore;
                        ServerThread._accountRepo.Update(ServerThread.Account);
                    }
                }
                ServerThread.IsPlaying = false;
            }
        }

        public void SpawnFood()
        {
            Gameplay.Instance.LevelGrid.SpawnFood();
            float posX = Gameplay.Instance.LevelGrid.FoodPositionX;
            float posY = Gameplay.Instance.LevelGrid.FoodPositionY;
            Gameplay.Instance.BroadcastMessage($"{(int)TransferType.Type.ACTION}{ServerThread.characters}{(int)Type.SPAWN_FOOD};{posX};{posY}");
        }
    }
}
