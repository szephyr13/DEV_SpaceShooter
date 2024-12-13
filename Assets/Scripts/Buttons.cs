using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject menu;
    [SerializeField] GameObject options;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject sourceSFX;
    [SerializeField] GameObject sourceBGM;

    //plays UI sound, BGM, reloads scene and unpauses
    public void ResetGame()
    {
        AudioManager.instance.PlaySFX("UISelect");
        AudioManager.instance.PlayBGM("Title");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1f;
    }

    //plays us sound, hides menu, uhnpauses, BGM.
    public void PlayFromMenu()
    {
        AudioManager.instance.PlaySFX("UISelect");
        mainMenu.SetActive(false);
        Time.timeScale = 1f;
        AudioManager.instance.PlayBGM("Enemies");
    }


    public void Resume()
    {
        AudioManager.instance.PlaySFX("UISelect");
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }


    //ui sound, hides menu, shows options
    public void Options()
    {
        AudioManager.instance.PlaySFX("UISelect");
        menu.SetActive(false);
        options.SetActive(true);
    }

    //ui sound, hides options, shows menu
    public void BackToMenu()
    {
        AudioManager.instance.PlaySFX("UISelect");
        options.SetActive(false);
        menu.SetActive(true);
    }

    //both attached to slide. finds audiosource if null and sets volume by slide
    public void SetVolumeBGM(float volume)
    {
        if (sourceBGM == null)
        {
            sourceBGM = GameObject.Find("BGM");
        }
        sourceBGM.GetComponent<AudioSource>().volume = volume;
    }

    public void SetVolumeSFX(float volume)
    {
        if (sourceSFX == null)
        {
            sourceBGM = GameObject.Find("SFX");
        }
        sourceSFX.GetComponent<AudioSource>().volume = volume;
    }
}
