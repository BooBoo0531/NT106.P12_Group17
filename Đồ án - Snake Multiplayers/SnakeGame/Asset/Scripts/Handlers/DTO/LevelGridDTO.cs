using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Handlers.DTO
{
    public class LevelGridDTO
    {
        public List<SnakeDTO> Snakes { get; set; }
        public float PositionX { get; set; } = 0;
        public float PositionY { get; set; } = 0;
        public float Width { get; set; } = 0;
        public float Height { get; set; } = 0;

        public float FoodPositionX { get; set; } = 0;

        public float FoodPositionY { get; set; } = 0;
    }
}
