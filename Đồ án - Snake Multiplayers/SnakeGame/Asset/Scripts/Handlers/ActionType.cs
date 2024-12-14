using Assets.Scripts;
using Newtonsoft.Json;
using Snake_Royale_Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Snake_Royale_Server.Handlers
{
    public class ActionType : TransferType
    {
        public Snake Snake { get; set; }
        public ActionType(Client client, Snake snake) : base(client)
        {
            Snake = snake;
        }

        public ActionType(Client client, int typeNum, Snake snake) : base(client, typeNum)
        {
            Snake = snake;
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
                    SpawnFood(actionData);
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
            string accountsJson = actionData.Split(";")[1];
            try
            {
                ListRankItemManager.Instance.Clear();
                var accounts = JsonConvert.DeserializeObject<List<AccountDTO>>(accountsJson);
                if (accounts != null && accounts.Count > 0)
                {
                    int i = 1;
                    foreach(var account in accounts)
                    {
                        ListRankItemManager.Instance.UpdateRankInfo(account.Username, i.ToString(), account.MaxScore.ToString(), account.GamePlayed.ToString());
                        i++;
                    }
                }
            } catch(Exception ex)
            {
                Debug.Log("Error load ranking: " + ex.ToString());
            }
        }

        public void AddScore(string actionData)
        {
            string username = actionData.Split(";")[1];
            long score = long.Parse(actionData.Split(";")[2]);
            //Debug.Log($"{username} score: {score}");
            GameHandler.Instance.LevelGrid.ChangeSnakeScore(username, score);
        }

        public void Eat(string actionData)
        {
            string username = actionData.Split(";")[1];
            int bodySize = int.Parse(actionData.Split(";")[2]);
            //Debug.Log($"{username} size: {bodySize}");
            GameHandler.Instance.LevelGrid.ChangeSnakeBodySize(username, bodySize);
        }

        public void Kill()
        {
            throw new NotImplementedException();
        }

        public void Die(string actionData)
        {
            string username = actionData.Split(";")[1];
            GameHandler.Instance.LevelGrid.RemoveSnake(username);
            //Debug.Log($"Snake {username} died");
        }

        public void SpawnFood(string actionData)
        {
            float posX = float.Parse(actionData.Split(';')[1]);
            float posY = float.Parse(actionData.Split(';')[2]);
            GameHandler.Instance.LevelGrid.SetFoodPosition(new Vector2(posX, posY));
        }
    }
}
