using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonHandler : MonoBehaviour
{
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
        StartCoroutine(BackButtonTimer());
    }

    public void RestartButton()
    {
        StartCoroutine(RestartButtonTimer());
    }

    IEnumerator StartButtonTimer()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Map");
    }

    IEnumerator ExitButtonTimer()
    {
        yield return new WaitForSeconds(2);
        Application.Quit();
    }

    IEnumerator BackButtonTimer()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Main Menu");
    }

    IEnumerator RestartButtonTimer()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
