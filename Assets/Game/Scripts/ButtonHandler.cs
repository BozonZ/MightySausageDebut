using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
    public static MusicManager mm;

    public void StartButton()
    {
        StartCoroutine(StartButtonTimer());
    }

    public void ExitButton()
    {
        StartCoroutine(ExitButtonTimer());
    }

    public void BackButton()
    {
        mm.PlayMenuAudio();
        SceneManager.LoadScene("Main Menu");
    }

    public void RestartButton()
    {
        mm.PlayGameAudio();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator StartButtonTimer()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Map");
        mm.PlayGameAudio();
    }

    IEnumerator ExitButtonTimer()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }
}
