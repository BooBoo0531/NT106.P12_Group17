using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;
using Assets.Scripts;
using UnityEngine.TextCore.Text;
using Snake_Royale_Server.Handlers;
using Assets.Scripts.Handlers;
using Newtonsoft.Json;

public class Snake : MonoBehaviour
{

    public enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    public enum State
    {
        Alive,
        Dead
    }
    private State state;
    private Direction gridMoveDirection;
    private Vector2 gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize = 0;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private SpriteRenderer spriteRenderer;
    private long score = 0;
    public GameAssets.SnakeColor Color { get; set; } = GameAssets.SnakeColor.green;
    
    public string Username { get; set; }

    public Client Client { get; set; }

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    public void SetupSnake(SnakeDTO snakeDTO)
    {
        Username = snakeDTO.Username;
        gridPosition = new Vector2(snakeDTO.PositionX, snakeDTO.PositionY);
        state = snakeDTO.CurrentState;
        gridMoveDirection = snakeDTO.CurrentDirection;
        if(snakeDTO.Score > 0)
        {
            score = snakeDTO.Score;
        }
        UpdateBodyPart(snakeDTO.SnakeBodySize);
        if(snakeDTO.Color != null)
        {
            Color = snakeDTO.Color.Value;
            spriteRenderer.sprite = GameAssets.instance.GetSnakeHeadSprite((int)Color);
        }
    }

    private void Awake()
    {
        gridPosition = new Vector2(5, 5);
        gridMoveTimerMax = .2f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = GameAssets.instance.GetSnakeHeadSprite((int)Color);
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
        state = State.Alive;
        Client = transform.GetComponent<Client>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        switch (state)
        {
            case State.Alive:
                if (Client != null)
                {
                    HandleInput();
                    HandleGridMovement();
                } else
                {
                    HandleGridMovement();
                }
                break;
            case State.Dead:
                break;
        }   
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Client != null && collision.gameObject.tag == "Food")
        {
            if(!GameHandler.Instance.LevelGrid.CurrentFoodEaten)
            {
                Object.Destroy(collision.gameObject);
                GameHandler.Instance.LevelGrid.CurrentFoodEaten = true;
                Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.SPAWN_FOOD};");
                CMDebug.TextPopupMouse("Snake Ate Food");
                score += 100;
                Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.ADD_SCORE};{Username};{score}");
                snakeBodySize++;
                CreateSnakeBodyPart();
                SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
                Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.EAT};{Username};{snakeBodySize}");
                GameHandler.Instance.UpdateRankedList();
            }
        }
    }

    public Direction GetDirection()
    {
        return gridMoveDirection;
    }
    public void SetDirection(Direction direction)
    {
        gridMoveDirection = direction;
        //Debug.Log(direction.ToString());
    }
    public void SetDirection(Direction direction, Vector3 position)
    {
        gridMoveDirection = direction;
        gridPosition = position;
        //Debug.Log(direction.ToString());
    }

    public void SetScore(long score)
    {
        this.score = score;
    }

    public long GetScore()
    {
        return score;
    }

    public void OnDestroy()
    {
        if(snakeBodyPartList != null && snakeBodyPartList.Count > 0)
        {
            foreach (var body in snakeBodyPartList)
            {
                Destroy(body.BodyPart);
            }
            snakeBodyPartList.Clear();
            snakeBodyPartList = null;
        }
        snakeMovePositionList.Clear();
        snakeMovePositionList = null;
    }
    private void HandleInput()
    {
        if (Client == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }
        } else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        } else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }
        } else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }
        }
    }

    public void HandleGridMovement()
    {
        if (gridPosition == null || GameHandler.Instance.LevelGrid == null || levelGrid == null)
        {
            return;
        }
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;
            SoundManager.PlaySound(SoundManager.Sound.SnakeMove);
            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            if (Client != null)
            {
                switch (gridMoveDirection)
                {
                    case Direction.Up:
                        Client.sendData($"{(int)TransferType.Type.MOVE}" + Client.characters + $"{(int)MoveType.Type.Up};{Username};{gridPosition.x};{gridPosition.y}");
                        break;
                    case Direction.Down:
                        Client.sendData($"{(int)TransferType.Type.MOVE}" + Client.characters + $"{(int)MoveType.Type.Down};{Username};{gridPosition.x};{gridPosition.y}");
                        break;
                    case Direction.Left:
                        Client.sendData($"{(int)TransferType.Type.MOVE}" + Client.characters + $"{(int)MoveType.Type.Left};{Username};{gridPosition.x};{gridPosition.y}");
                        break;
                    case Direction.Right:
                        Client.sendData($"{(int)TransferType.Type.MOVE}" + Client.characters + $"{(int)MoveType.Type.Right};{Username};{gridPosition.x};{gridPosition.y}");
                        break;
                }
            }

            Vector2Int gridMoveDirectionVector;
            switch (gridMoveDirection)
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            gridPosition += gridMoveDirectionVector;

            gridPosition = levelGrid.ValidateGridPosition(gridPosition);
            //bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            //if (snakeAteFood)
            //{
            //    // Snake ate food, grow body
            //    snakeBodySize++;
            //    CreateSnakeBodyPart();
            //    SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
            //    Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.EAT};{Username};{snakeBodySize}");
            //}

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }
            /*for (int i = 0; i < snakeMovePositionList.Count; i++) {
                Vector2Int snakeMovePosition = snakeMovePositionList[i];
                World_Sprite worldSprite = World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .5f, Color.white);
                FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            }*/

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector) - 90);

            UpdateSnakeBodyParts();
            if(snakeBodyPartList != null && snakeBodyPartList.Count > 0)
            {
                foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
                {
                    if (snakeBodyPart == null)
                    {
                        continue;
                    }
                    Vector2 snakeBodyPartPosition = snakeBodyPart.GetGridPosition();
                    if (snakeBodyPartPosition.x != -99 && gridPosition == snakeBodyPartPosition)
                    {
                        //CMDebug.TextPopup("DEAD!", transform.position);
                        state = State.Dead;
                        SoundManager.PlaySound(SoundManager.Sound.SnakeDie);
                        GameHandler.Instance.SnakeDied(Username);
                    }
                }
            }
        }
    }
    
    public void UpdateBodyPart(int bodySize)
    {
        int changeSize = bodySize - snakeBodySize;
        if(changeSize > 0)
        {
            snakeBodySize = bodySize;
            for(int i = 0; i < changeSize; i++)
            {
                CreateSnakeBodyPart();
            }
            UpdateSnakeBodyParts();
            SoundManager.PlaySound(SoundManager.Sound.SnakeEat);
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count, (int)Color));
    }

    private void UpdateSnakeBodyParts()
    {
        if(snakeBodyPartList.Count == 0)
        {
            return;
        }
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            if(i >= snakeMovePositionList.Count)
            {
                break;
            }
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }


    private float GetAngleFromVector(Vector2 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2 GetGridPosition()
    {
        return gridPosition;
    }

    // Return the full list of positions occupied by the snake: Head + Body
    public List<Vector2> GetFullSnakeGridPositionList()
    {
        List<Vector2> gridPositionList = new List<Vector2>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }




    /*
     * Handles a Single Snake Body Part
     * */
    private class SnakeBodyPart
    {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public GameObject BodyPart { get; set; }

        public SnakeBodyPart(int bodyIndex, int color)
        {
            BodyPart = new GameObject("SnakeBody", typeof(SpriteRenderer));
            BodyPart.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.GetSnakeBodySprite(color);
            BodyPart.GetComponent<SpriteRenderer>().sortingOrder = -1 - bodyIndex;
            transform = BodyPart.transform;
            transform.position = new Vector3(-99, -99);
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;

            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:
                case Direction.Up: // Currently going Up
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0;
                            break;
                        case Direction.Left: // Previously was going Left
                            angle = 0 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Right: // Previously was going Right
                            angle = 0 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                    }
                    break;
                case Direction.Down: // Currently going Down
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180;
                            break;
                        case Direction.Left: // Previously was going Left
                            angle = 180 - 45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                        case Direction.Right: // Previously was going Right
                            angle = 180 + 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Left: // Currently going to the Left
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = +90;
                            break;
                        case Direction.Down: // Previously was going Down
                            angle = 180 - 45;
                            transform.position += new Vector3(-.2f, .2f);
                            break;
                        case Direction.Up: // Previously was going Up
                            angle = 45;
                            transform.position += new Vector3(-.2f, -.2f);
                            break;
                    }
                    break;
                case Direction.Right: // Currently going to the Right
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90;
                            break;
                        case Direction.Down: // Previously was going Down
                            angle = 180 + 45;
                            transform.position += new Vector3(.2f, .2f);
                            break;
                        case Direction.Up: // Previously was going Up
                            angle = -45;
                            transform.position += new Vector3(.2f, -.2f);
                            break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2 GetGridPosition()
        {
            if(snakeMovePosition == null)
            {
                return new Vector2(-99, -99);
            }
            return snakeMovePosition.GetGridPosition();
        }
    }



    /*
     * Handles one Move Position from the Snake
     * */
    private class SnakeMovePosition
    {

        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2 gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2 gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2 GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }

    }

}
