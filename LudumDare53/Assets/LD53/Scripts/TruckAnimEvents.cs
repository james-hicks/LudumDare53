using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TruckAnimEvents : MonoBehaviour
{
    [SerializeField] private TruckCollider truckCollider;
    [SerializeField] private AudioSource truckExitSource;
    [SerializeField] private AudioSource truckEnterSource;
    public void EndLoad()
    {
        truckCollider.EndOrder();
    }

    public void SpawnNewLoad()
    {
        truckCollider.StartNewLoad();
    }

    public void SwapMaterial()
    {
        truckCollider.SwapMaterial();
    }

    public void TruckExit()
    {
        truckExitSource.volume = 1;
        truckExitSource.Play();
        StartCoroutine(StartFade(truckExitSource, 4f, 0f));
    }

    public void TruckArrive()
    {
        truckEnterSource.Play();
        truckEnterSource.time = 20f;
        truckEnterSource.volume = 0f;
        StartCoroutine(StartFade(truckEnterSource, 2f, 1));
    }

    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
