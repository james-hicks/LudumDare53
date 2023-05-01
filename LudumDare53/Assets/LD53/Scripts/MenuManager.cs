using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject PauseMenuUI;
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Button defaultSelect;
    public bool isGamePaused = false;
    public void PauseGame()
    {
        Time.timeScale = 0f;
        isGamePaused = true;

        PauseMenuUI.SetActive(true);
        defaultSelect.Select();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isGamePaused = false;
        PauseMenuUI.SetActive(false);
    }

    public void LoadScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

    public void QuitGame()
    {
        Debug.LogWarning("Quitting Game...");
        Application.Quit();
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("Master", Mathf.Log10(volume) * 20);
    }

    public void SetForkliftVolume(float volume) 
    {
        audioMixer.SetFloat("Forklift Audio", Mathf.Log10(volume) * 20);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }
}
