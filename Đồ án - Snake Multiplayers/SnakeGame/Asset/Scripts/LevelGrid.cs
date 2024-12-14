using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using Assets.Scripts;
using Snake_Royale_Server.Handlers;
using System.Linq;
using System.Runtime.CompilerServices;

public class LevelGrid
{
    private Vector2 foodGridPosition;
    private float x, y;
    private float width, height;
    private GameObject foodGameObject;
    private GameObject playField;
    private SpriteRenderer playFieldRenderer;
    //private Snake snake;
    public List<Snake> Snakes { get; set; } = new List<Snake>();

    public bool CurrentFoodEaten { get; set; } = false;


    public Vector2 GetPos()
    {
        return new Vector2(x, y);
    }

    public Vector2 GetSize()
    {
        return new Vector2(width, height);
    }

    public LevelGrid(GameObject playField)
    {
        this.playField = playField;
        playFieldRenderer = playField.GetComponent<SpriteRenderer>();
        x = playFieldRenderer.bounds.center.x;
        y = playFieldRenderer.bounds.center.y;
        width = x + playFieldRenderer.bounds.size.x;
        height = y + playFieldRenderer.bounds.size.y;
        //Client.Instance.sendData($"{(int)TransferType.Type.AUTHEN}{Client.characters}{(int)AuthenType.Type.StartGame};{x};{y};{width};{height}");
    }

    public void ChangeSnakeScore(string username, long score)
    {
        try
        {
            if (Snakes.Any(s => s.Username == username))
            {
                var snake = Snakes.First(s => s.Username == username);
                if (snake == null)
                {
                    return;
                }
                snake.SetScore(score);
                GameHandler.Instance.UpdateRankedList();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
        }
    }

    public void ChangeSnakeBodySize(string username, int size)
    {
        try
        {
            if (Snakes.Any(s => s.Username == username))
            {
                var snake = Snakes.First(s => s.Username == username);
                if (snake == null)
                {
                    return;
                }
                snake.UpdateBodyPart(size);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
        }
    }

    public void RemoveSnake(string username)
    {
        try
        {
            if (Snakes.Any(s => s.Username == username))
            {
                var snake = Snakes.First(s => s.Username == username);
                if (snake == null)
                {
                    return;
                }
                GameHandler.Instance.DestroyRankList(username);
                Snakes.Remove(snake);
                Object.Destroy(snake.gameObject);
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
        }
    }

    public void MoveSnake(string username, float posX, float posY, int moveType)
    {
        try
        {
            if (Snakes.Any(s => s.Username == username))
            {
                var snake = Snakes.First(s => s.Username == username);
                if (snake == null)
                {
                    return;
                }
                var nPos = new Vector3(posX, posY, 0);
                switch (moveType)
                {
                    case (int)MoveType.Type.Up:
                        snake.SetDirection(Snake.Direction.Up, nPos);
                        break;
                    case (int)MoveType.Type.Down:
                        snake.SetDirection(Snake.Direction.Down, nPos);
                        break;
                    case (int)MoveType.Type.Left:
                        snake.SetDirection(Snake.Direction.Left, nPos);
                        break;
                    case (int)MoveType.Type.Right:
                        snake.SetDirection(Snake.Direction.Right, nPos);
                        break;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
        }
    }

    public void Reset()
    {
        try
        {
            Object.Destroy(foodGameObject);
            foreach (var snake in Snakes)
            {
                GameHandler.Instance.DestroyRankList(snake.Username);
                Object.Destroy(snake.gameObject);
            }
            Snakes.Clear();
        } catch (System.Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
        }
    }

    public void WaitToRetry(string username)
    {
        try
        {
            Object.Destroy(foodGameObject);
            foreach (var snake in Snakes.ToList())
            {
                if(username != snake.Username)
                {
                    Object.Destroy(snake.gameObject);
                    Snakes.Remove(snake);
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Exception: {ex.Message}");
        }
    }
    public void Setup(Snake snake)
    {
        if(Snakes.Count == 0)
        {
            Snakes.Add(snake);
            return;
        }
        if (!Snakes.Any(s => s.Username == snake.Username))
        {
            Snakes.Add(snake);
        }
    }

    public GameObject GetFoodObject()
    {
        return foodGameObject;
    }

    public void SetFoodPosition(Vector2 foodGridPosition)
    {
        if (!CurrentFoodEaten && foodGameObject != null)
        {
            Object.Destroy(foodGameObject);
        }
        this.foodGridPosition = foodGridPosition;
        CurrentFoodEaten = false;
        SpawnFood();
    }
    public void SpawnFood()
    {
        if (foodGridPosition == null || CurrentFoodEaten)
        {
            return;
        }
        //do
        //{
        //    foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        //} while (snake.GetFullSnakeGridPositionList().IndexOf(foodGridPosition) != -1);
        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        var spriteRenderer = foodGameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameAssets.instance.foodSprite;
        foodGameObject.AddComponent<BoxCollider2D>();
        foodGameObject.tag = "Food";
        BoxCollider2D boxCollider = foodGameObject.GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            boxCollider.size = spriteRenderer.sprite.bounds.size;
            boxCollider.offset = Vector2.zero;
        }
        foodGameObject.transform.position = new Vector3(foodGridPosition.x, foodGridPosition.y, 0);
    }

    //public bool TrySnakeEatFood(Vector2 snakeGridPosition)
    //{
    //    if (snakeGridPosition == foodGridPosition)
    //    {
    //        Object.Destroy(foodGameObject);
    //        CurrentFoodEaten = true;
    //        Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.SPAWN_FOOD};");
    //        CMDebug.TextPopupMouse("Snake Ate Food");
    //        GameHandler.Instance.AddScore();
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
    public Vector2 ValidateGridPosition(Vector2 gridPosition)
    {
        // Calculate the bounds of the grid
        float minX = (x - width) / 2 + x;
        float minY = (y + height) / 2;
        float maxX = (x + width) / 2;
        float maxY = (y - height) / 2 + y;
        // Wrap X position
        if (gridPosition.x < minX)
        {
            gridPosition.x = maxX;
        }
        else if (gridPosition.x > maxX)
        {
            gridPosition.x = minX;
        }
        // Wrap Y position
        if (gridPosition.y > minY)
        {
            gridPosition.y = maxY;
        }
        else if (gridPosition.y < maxY)
        {
            gridPosition.y = minY;
        }
        return gridPosition;
    }

    public float GetWidth()
    {
        return width;
    }
    public float GetHeight()
    {
        return height;
    }
    public Vector2 GetLevelGridPosition()
    {
        return new Vector2(x, y);
    }
}