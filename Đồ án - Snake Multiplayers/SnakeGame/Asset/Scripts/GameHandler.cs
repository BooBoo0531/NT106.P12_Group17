using Assets.Scripts;
using Assets.Scripts.Handlers;
using Assets.Scripts.Handlers.DTO;
using CodeMonkey;
using Snake_Royale_Server.Handlers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class GameHandler : MonoBehaviour {
    public static GameHandler Instance
    {
        get
        {
            return instance;
        }
    }
    private static GameHandler instance;
    [SerializeField] private Snake defaultSnake;
    [SerializeField] private GameObject playField;
    [SerializeField] private ScoreWindow scoreWindow;
    public LevelGrid LevelGrid { get; set; }

    public bool isGameStarted { get; set; } = false;

    public bool isViewRanking { get; set; } = false;

    private void Awake()
    {
        instance = this;
    }
    private void Start() {
        Time.timeScale = 0f;
    }

    public Snake GetDefaultSnake()
    {
        return defaultSnake;
    }

    public void StartGame()
    {
        LevelGrid = new LevelGrid(playField);
    }

    public void SetLevelGrid(LevelGridDTO levelGridDTO, Snake snake)
    {
        if(LevelGrid != null)
        {
            LevelGrid.Reset();
        }
        LevelGrid.SetFoodPosition(new Vector2(levelGridDTO.FoodPositionX, levelGridDTO.FoodPositionY));
        if (levelGridDTO.Snakes != null && levelGridDTO.Snakes.Count > 0)
        {
            foreach(var snakeDTO in levelGridDTO.Snakes)
            {
                snakeDTO.Color = GameAssets.instance.GetSnakeColor();
                if (snakeDTO.Username == snake.Username)
                {
                    snake.SetupSnake(snakeDTO);
                    LevelGrid.Setup(snake);
                    snake.Setup(LevelGrid);
                } else
                {
                    var otherSnake = Instantiate(defaultSnake, new Vector3(snakeDTO.PositionX, snakeDTO.PositionY, 0), new Quaternion());
                    var destroyClient = otherSnake.GetComponent<Client>();
                    Destroy(destroyClient);
                    otherSnake.Client = null;
                    otherSnake.SetupSnake(snakeDTO);
                    LevelGrid.Setup(otherSnake);
                    otherSnake.Setup(LevelGrid);
                }
            }
            isGameStarted = true;
            Time.timeScale = 1f;
            UpdateRankedList();
        }
    }

    public void DestroyRankList(string username)
    {
        scoreWindow.DestroyPlayerRank(username);
    }

    public void UpdateRankedList()
    {
        if(LevelGrid == null)
        {
            return;
        }
        if(LevelGrid.Snakes != null && LevelGrid.Snakes.Count > 0)
        {
            LevelGrid.Snakes = LevelGrid.Snakes.OrderByDescending(x => x.GetScore()).Take(5).ToList();
            int i = 1;
            foreach(var snake in LevelGrid.Snakes)
            {
                scoreWindow.UpdateRankInfo(snake.Username, i + "", snake.GetScore().ToString());
                i++;
            }
        }
    }

    public void NewSnakeJoin(SnakeDTO snakeDTO)
    {
        snakeDTO.Color = GameAssets.instance.GetSnakeColor();
        var otherSnake = Instantiate(defaultSnake, new Vector3(snakeDTO.PositionX, snakeDTO.PositionY, 0), new Quaternion());
        var destroyClient = otherSnake.GetComponent<Client>();
        Destroy(destroyClient);
        otherSnake.Client = null;
        otherSnake.SetupSnake(snakeDTO);
        LevelGrid.Setup(otherSnake);
        otherSnake.Setup(LevelGrid);
        UpdateRankedList();
    }

    private void Update()
    {
        if (isGameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isGamePaused())
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (!isViewRanking)
                {
                    Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.LOAD_RANKING};");
                    ListRankItemManager.Instance.gameObject.SetActive(true);
                    isViewRanking = true;
                }
                else
                {
                    ListRankItemManager.Instance.gameObject.SetActive(false);
                    isViewRanking = false;
                }
            }
        }
    }
    public void SnakeDied(string username)
    {
        GameOverWindow.ShowStatic();
        Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.DIE};{username}");
        LevelGrid.WaitToRetry(username);
    }

    public void ResumeGame()
    {
        PauseWindow.HideStatic();
        Time.timeScale = 1f;
    }
    public void PauseGame()
    {
        PauseWindow.ShowStatic();
        Time.timeScale = 0f;
    }
    public bool isGamePaused()
    {
        return Time.timeScale == 0f;
    }
}
