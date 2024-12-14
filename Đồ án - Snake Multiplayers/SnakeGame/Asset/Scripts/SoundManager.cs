using UnityEngine;
using CodeMonkey.Utils;

public static class SoundManager
{

    public enum Sound
    {
        SnakeMove,
        SnakeDie,
        SnakeEat,
        ButtonClick,
        ButtonOver,
    }
    
    public static GameObject SoundObject { get; set; }

    public static void PlaySound(Sound sound)
    {
        if(SoundObject != null)
        {
            Object.Destroy(SoundObject);
        }
        SoundObject = new GameObject("Sound");
        AudioSource audioSource = SoundObject.AddComponent<AudioSource>();
        var audioClip = GetAudioClip(sound);
        audioSource.PlayOneShot(audioClip);
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (GameAssets.SoundAudioClip soundAudioClip in GameAssets.instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound " + sound + " not found!");
        return null;
    }

    public static void AddButtonSounds(this Button_UI buttonUI)
    {
        buttonUI.MouseOverOnceFunc += () => SoundManager.PlaySound(Sound.ButtonOver);
        buttonUI.ClickFunc += () => SoundManager.PlaySound(Sound.ButtonClick);
    }

}
