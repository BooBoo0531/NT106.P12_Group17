using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Snake_Royale_Server.Handlers
{
    public partial class MoveType : TransferType
    {
        private Snake snake;
        public MoveType(Client client, Snake snake) : base(client)
        {
            this.snake = snake;
        }

        public MoveType(Client client, int typeNum, Snake snake) : base(client, typeNum)
        {
            this.snake = snake;
        }

        public enum Type
        {
            Up = 1,
            Down = 2,
            Left = 3,
            Right = 4
        }
        public override string GetType()
        {
            switch (TypeNum)
            {
                case (int)Type.Up:
                    return Type.Up.ToString();
                case (int)Type.Down:
                    return Type.Down.ToString();
                case (int)Type.Left:
                    return Type.Left.ToString();
                case (int)Type.Right:
                    return Type.Right.ToString();
                default:
                    return DEFAULT_TYPE;
            }
        }

        public override void HandleAction(string actionData)
        {
            string username = actionData.Split(';')[1];
            float posX = float.Parse(actionData.Split(';')[2]);
            float posY = float.Parse(actionData.Split(';')[3]);
            GameHandler.Instance.LevelGrid.MoveSnake(username, posX, posY, TypeNum);
            if (snake.Username != username)
            {
                return;
            }
            switch (TypeNum)
            {
                case (int)Type.Up:
                    MoveUp();
                    break;
                case (int)Type.Down:
                    MoveDown();
                    break;
                case (int)Type.Left:
                    MoveLeft();
                    break;
                case (int)Type.Right:
                    MoveRight();
                    break;
                default:
                    break;
            }
        }

        public void MoveUp()
        {
            if (snake.GetDirection() != Snake.Direction.Down)
            {
                snake.SetDirection(Snake.Direction.Up);
            }
            // Move the snake up
        }

        public void MoveDown()
        {
            if (snake.GetDirection() != Snake.Direction.Up)
            {
                snake.SetDirection(Snake.Direction.Down);
            }
            // Move the snake down
        }

        public void MoveLeft()
        {
            if (snake.GetDirection() != Snake.Direction.Right)
            {
                snake.SetDirection(Snake.Direction.Left);
            }
            // Move the snake left
        }

        public void MoveRight()
        {
            if (snake.GetDirection() != Snake.Direction.Left)
            {
                snake.SetDirection(Snake.Direction.Right);
            }
            // Move the snake right
        }
    }
}
