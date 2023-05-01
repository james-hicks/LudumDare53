using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SoundManager : MonoBehaviour
{
    public UnityEvent OnReverse;
    public UnityEvent OnReverseStop;

    public UnityEvent OnBrake;
    public UnityEvent OnBrakeStop;

    public UnityEvent OnForklift;
    public UnityEvent OnForkliftStop;

    public UnityEvent CompleteOrder;
    public UnityEvent FailOrder;

    public AudioSource mainMusic;
    [SerializeField] private AudioClip[] musics;
    private int index = 1;

    private void Update()
    {
        if (mainMusic.time == mainMusic.clip.length)
        {
            mainMusic.clip = musics[index];
            mainMusic.Play();
            if(index == 1)
            {
                index = 2;
            } else if(index == 2)
            {
                index = 0;
            }
            else
            {
                index = 1;
            }
        }
    }
}
