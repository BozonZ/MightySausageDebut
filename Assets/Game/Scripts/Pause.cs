using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isActive;

    void Start()
    {
        pauseMenu.SetActive(false);
        isActive = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isActive == false)
            {
                Time.timeScale = 0;
                isActive = true;
                pauseMenu.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                isActive = false;
                pauseMenu.SetActive(false);
            }
        }
    }
}
