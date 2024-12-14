using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Snake_Royale_Server.Handlers
{
    public partial class MoveType : TransferType
    {
        public MoveType(ServerThread serverThread) : base(serverThread)
        {
        }

        public MoveType(ServerThread serverThread, int typeNum) : base(serverThread, typeNum)
        {
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
            Gameplay.Instance.LevelGrid.MoveSnake(username, posX, posY);
            switch (TypeNum)
            {
                case (int)Type.Up:
                    MoveUp(username, posX, posY);
                    break;
                case (int)Type.Down:
                    MoveDown(username, posX, posY);
                    break;
                case (int)Type.Left:
                    MoveLeft(username, posX, posY);
                    break;
                case (int)Type.Right:
                    MoveRight(username, posX, posY);
                    break;
                default:
                    break;
            }
        }

        public void MoveUp(string username, float posX, float posY)
        {
            //ServerThread.GetServerForm().AddServerInfo("Move Up");
            Gameplay.Instance.BroadcastMessageExcept($"{(int)TransferType.Type.MOVE}" + ServerThread.characters + $"{(int)Type.Up};{username};{posX};{posY}", ServerThread);
            // Move the snake up
        }

        public void MoveDown(string username, float posX, float posY)
        {
            //ServerThread.GetServerForm().AddServerInfo("Move Down");
            Gameplay.Instance.BroadcastMessageExcept($"{(int)TransferType.Type.MOVE}" + ServerThread.characters + $"{(int)Type.Down};{username};{posX};{posY}", ServerThread);
            // Move the snake down
        }

        public void MoveLeft(string username, float posX, float posY)
        {
            //ServerThread.GetServerForm().AddServerInfo("Move Left");
            Gameplay.Instance.BroadcastMessageExcept($"{(int)TransferType.Type.MOVE}" + ServerThread.characters + $"{(int)Type.Left};{username};{posX};{posY}", ServerThread);
            // Move the snake left
        }

        public void MoveRight(string username, float posX, float posY)
        {
            //ServerThread.GetServerForm().AddServerInfo("Move Right");
            Gameplay.Instance.BroadcastMessageExcept($"{(int)TransferType.Type.MOVE}" + ServerThread.characters + $"{(int)Type.Right};{username};{posX};{posY}", ServerThread);
            // Move the snake right
        }
    }
}
