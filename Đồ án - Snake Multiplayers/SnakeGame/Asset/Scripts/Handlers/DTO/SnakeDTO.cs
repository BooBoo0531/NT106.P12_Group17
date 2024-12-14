using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Handlers
{
    public class SnakeDTO
    {
        public string Username { get; set; }
        public Snake.State CurrentState { get; set; }
        public Snake.Direction CurrentDirection { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public int SnakeBodySize { get; set; }

        public long Score { get; set; }

        public GameAssets.SnakeColor? Color { get; set; } = GameAssets.SnakeColor.green;
    }
}
