using Snake_Royale_Server;
using Snake_Royale_Server.Handlers;
using System;

public class LevelGridDTO
{
    public List<SnakeDTO> Snakes { get; } = new List<SnakeDTO>();
    public float PositionX { get; set; } = 0;
    public float PositionY { get; set; } = 0;
    public float Width { get; set; } = 0;
    public float Height { get; set; } = 0;

    public float FoodPositionX { get; set; } = 0;

    public float FoodPositionY { get; set; } = 0;


    public LevelGridDTO(float x, float y, float width, float height)
    {
        PositionX = x;
        PositionY = y;
        Width = width;
        Height = height;
        SpawnFood();
    }

    public SnakeDTO GetSnake(string username)
    {
        if(Snakes.Count == 0)
        {
            return null;
        }
        if(Snakes.Any(s => s.Username == username))
        {
            return Snakes.First(s => s.Username == username);
        }
        return null;
    }

    public void ChangeSnakeScore(string username, long score)
    {
        if(Snakes.Count == 0)
        {
            return;
        }
        if(Snakes.Any(s => s.Username == username))
        {
            var snake = Snakes.First(s => s.Username == username);
            if(snake == null)
            {
                return;
            }
            snake.Score = score;
        }
    }

    public void ChangeSnakeBodySize(string username, int size)
    {
        if(Snakes.Count == 0)
        {
            return;
        }
        if(Snakes.Any(s => s.Username == username))
        {
            var snake = Snakes.First(s => s.Username == username);
            if(snake == null)
            {
                return;
            }
            snake.SnakeBodySize = size;
        }
    }

    public void MoveSnake(string username, float posX, float posY)
    {
        if(Snakes.Count == 0)
        {
            return;
        }
        if(Snakes.Any(s => s.Username == username))
        {
            var snake = Snakes.First(s => s.Username == username);
            if(snake == null)
            {
                return;
            }
            snake.PositionX = posX;
            snake.PositionY = posY;
        }
    }
    public void RemoveSnake(string username)
    {
        if(Snakes.Count == 0)
        {
            return;
        }
        if (Snakes.Any(s => s.Username == username))
        {
            var snake = Snakes.First(s => s.Username == username);
            if (snake == null)
            {
                return;
            }
            Snakes.Remove(snake);
        } 
    }
    public void Setup(SnakeDTO snake)
    {
        var position = GetPosNotDuplicate();
        snake.PositionX = position.Item1;
        snake.PositionY = position.Item2;
        int i = 1;
        while(Snakes.Any(s => s.Username == snake.Username))
        {
            snake.Username = snake.Username + $" {i}";
            i++;
        }
        Snakes.Add(snake);
    }

    public (float, float) GetPosNotDuplicate()
    {
        var minX = (PositionX - Width) / 2 + PositionX;
        var minY = (PositionY + Height) / 2;
        var maxX = (PositionX + Width) / 2;
        var maxY = (PositionY - Height) / 2 + PositionY;
        float spawnX = RandomFloat(minX, maxX);
        float spawnY = RandomFloat(minY, maxY);
        while (true)
        {
            if(Snakes.Count > 0)
            {
                bool isDuplicate = false;
                foreach (var snakeInList in Snakes)
                {
                    if ((snakeInList.PositionX == spawnX && snakeInList.PositionY == spawnY)
                        || (FoodPositionX == spawnX && FoodPositionY == spawnY))
                    {
                        spawnX = RandomFloat(minX, maxX);
                        spawnY = RandomFloat(minY, maxY);
                        isDuplicate = true;
                        break;
                    }
                }
                if(!isDuplicate)
                {
                    break;
                }
            } else
            {
                break;
            }
        }
        return (spawnX, spawnY);
    }

    private float RandomFloat(float minValue, float maxValue)
    {
        var random = new Random();
        return (float)(random.NextDouble() * (maxValue - minValue) + minValue);
    }

    public void SpawnFood()
    {
        var minX = (PositionX - Width) / 2 + PositionX;
        var minY = (PositionY + Height) / 2;
        var maxX = (PositionX + Width) / 2;
        var maxY = (PositionY - Height) / 2 + PositionY;
        FoodPositionX = RandomFloat(minX, maxX);
        FoodPositionY = RandomFloat(minY, maxY);
    }
}
