using Assets.Scripts;
using CodeMonkey.Utils;
using Snake_Royale_Server.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverWindow : MonoBehaviour
{
    private static GameOverWindow instance;
    private void Awake()
    {
        instance = this;
        transform.Find("retryButton").GetComponent<Button_UI>().ClickFunc = () =>
        {
            GameCacheData.Instance.isRetry = true;
            GameHandler.Instance.LevelGrid.Reset();
            Loader.Load(Loader.Scene.GameScene);
        };
        Hide();
    }
    private void Show()
    {
        gameObject.SetActive(true);
    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }

    public static void ShowStatic()
    {
        instance.Show();
    }
}
