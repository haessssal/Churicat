using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioClip sceneMusic;

    void Start()
    {
        GameManager.instance.ChangeBackgroundMusic(sceneMusic);
    }
}
