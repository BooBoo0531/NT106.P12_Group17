using Assets.Scripts;
using CodeMonkey.Utils;
using Snake_Royale_Server.Handlers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWindow : MonoBehaviour
{
    private static PauseWindow instance;
    private void Awake()
    {
        instance = this;
        transform.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        transform.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        transform.Find("resumeBtn").GetComponent<Button_UI>().ClickFunc = () => { GameHandler.Instance.ResumeGame(); };
        transform.Find("mainMenuBtn").GetComponent<Button_UI>().ClickFunc = () => {
            string username = Client.Instance.Snake.Username;
            Client.Instance.sendData($"{(int)TransferType.Type.ACTION}{Client.characters}{(int)ActionType.Type.DIE};{username}");
            GameHandler.Instance.LevelGrid.RemoveSnake(username);
            Loader.Load(Loader.Scene.MainMenu); 
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

    public static void HideStatic()
    {
        instance.Hide();
    }
    public static void ShowStatic()
    {
        instance.Show();
    }
}
