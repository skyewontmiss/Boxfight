using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuScript : MonoBehaviour
{
    public GameObject PauseMenu;
    public bool paused = false;
    public bool animating;
    public Animator pauseMenuAnimator;
    void Start()
    {
        PauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PauseMenuChecker();
    }

    void PauseMenuChecker()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKey(KeyCode.Alpha0) || Input.GetKey(KeyCode.Keypad0))
        {
            if (!animating)
            {
                if (paused)
                {
                    CloseMenu();
                }
                else if (!paused)
                {
                    OpenMenu();
                }
            }
        }
    }

    void OpenMenu()
    {
        Time.timeScale = 0;
        PauseMenu.SetActive(true);
        //animating = true;// if functions are compressed
        //if (pauseMenuAnimator != null) { pauseMenuAnimator.Play("PauseMenuOpen", 0, 0f); }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        animating = false;
        paused = true;
    }

    void CloseMenu()
    {
        Time.timeScale = 1f;
        PauseMenu.SetActive(false);
       // animating = true;// if functions are compressed
       // if (pauseMenuAnimator != null) { pauseMenuAnimator.Play("PauseMenuClose", 0, 0f); }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        animating = false;
        paused = false;
    }

    public void Resumme()
    {

        CloseMenu();
    }
    public void Menu()
    {
        Debug.Log("test menu button");
        SceneManager.LoadScene("i donk know the menu scene name so type here");
    }
    public void Quit()
    {
        print("quit test");
        Application.Quit();
    }
}
