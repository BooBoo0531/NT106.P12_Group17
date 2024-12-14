/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public enum SnakeColor
    {
        green = 0,
        blue = 1,
        lightblue = 2,
        gray = 3
    }
    public static GameAssets instance;

    private void Awake()
    {
        instance = this;
    }
    public List<Sprite> snakeHeadSprite;
    public List<Sprite> snakeBodySprite;
    public Sprite foodSprite;
    public SoundAudioClip[] soundAudioClipArray;

    public int SnakeColorIndex { get; set; } = 0;
    [Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    public SnakeColor GetSnakeColor()
    {
        int index = SnakeColorIndex;
        SnakeColor color = SnakeColor.green;
        switch (index)
        {
            case (int)SnakeColor.green:
                color = SnakeColor.green;
                break;
            case (int)SnakeColor.blue:
                color = SnakeColor.blue;
                break;
            case (int)SnakeColor.lightblue:
                color = SnakeColor.lightblue;
                break;
            case (int)SnakeColor.gray:
                color = SnakeColor.gray;
                break;
        }
        SnakeColorIndex++;
        if(SnakeColorIndex >= 4)
        {
            SnakeColorIndex = 0;
        }
        return color;
    }

    public Sprite GetSnakeHeadSprite(int colorIndex)
    {
        return snakeHeadSprite[colorIndex];
    }

    public Sprite GetSnakeBodySprite(int colorIndex)
    {
        return snakeBodySprite[colorIndex];
    }
}
