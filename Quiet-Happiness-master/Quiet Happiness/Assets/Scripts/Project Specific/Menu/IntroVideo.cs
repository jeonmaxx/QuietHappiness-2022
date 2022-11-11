using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class IntroVideo : MonoBehaviour
{
    private VideoPlayer video;

    void Awake()
    {
        video = GetComponent<VideoPlayer>();
        string key = ModuleManager.GetModule<PlayerPrefKeys>().getPrefereceKey(PlayerPrefKeys.EFFECTS_VOLUME);
        if (PlayerPrefs.HasKey(key)) {
            video.SetDirectAudioVolume(0, PlayerPrefs.GetFloat(key));
        }
        else
        {
            video.SetDirectAudioVolume(0, 0.5f);
        }
        video.Play();
        video.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene("Menu");
    }
}
