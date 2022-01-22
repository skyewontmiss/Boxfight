using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyOptionsMenu : MonoBehaviour
{
    public Slider MouseLook, cameraFOVSlider;

    public TMP_Text mouseText;

    //menu settings text components
    public TMP_Text FullscreenText, VSyncText, PostProcessingText, textChatText, cameraFOVText, fpsCounterText;
    public Animator popUpExperimental;
    public TMP_Dropdown resolutionDropdown, cameraModeDropdown;
    bool fullscreenBoolForRes;
    public TMP_Text usernameOptionsPlaceholderText, usernameOptionsSettingHeaderText;
    public TMP_InputField usernameOptionsInputField;

    private void Start()
    {
        LoadAllPlayerPrefs();
        fullscreenBoolForRes = false;
    }

    private void LoadAllPlayerPrefs()
    {

        //mouse settings
        if (PlayerPrefs.HasKey("Mouse"))
        {
            MouseLook.value = PlayerPrefs.GetInt("Mouse");
        }
        else
        {
            MouseLook.value = 1;
            PlayerPrefs.SetInt("Mouse", 1);
            PlayerPrefs.Save();
        }

        //textchat
        if (PlayerPrefs.HasKey("Text Chat"))
        {
            if (PlayerPrefs.GetInt("Text Chat") == 0)
            {
                textChatText.text = "off";
            }
            else if (PlayerPrefs.GetInt("Text Chat") == 1)
            {
                textChatText.text = "on";
            }
        }
        else
        {
            PlayerPrefs.SetInt("Text Chat", 0);
            PlayerPrefs.Save();
        }



        //fps counter
        if (PlayerPrefs.HasKey("FPS Counter"))
        {
            if (PlayerPrefs.GetInt("FPS Counter") == 0)
            {
                fpsCounterText.text = "off";
            }
            else if (PlayerPrefs.GetInt("FPS Counter") == 1)
            {
                fpsCounterText.text = "on";
            }
        }
        else
        {
            PlayerPrefs.SetInt("FPS Counter", 0);
            PlayerPrefs.Save();
        }



        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {
            //fullscreen
            if (PlayerPrefs.HasKey("Fullscreen"))
            {
                if (PlayerPrefs.GetInt("Fullscreen") == 0)
                {
                    //turn fullscreen off and make the text go off
                    Screen.fullScreen = false;
                    FullscreenText.text = "off";
                    fullscreenBoolForRes = false;
                }
                else if (PlayerPrefs.GetInt("Fullscreen") == 1)
                {
                    //turn fullscreen on and make text go on
                    Screen.fullScreen = true;
                    FullscreenText.text = "on";
                    fullscreenBoolForRes = true;
                }
            } 

            //resolution
            if (PlayerPrefs.HasKey("Resolution"))
            {
                if (PlayerPrefs.GetInt("Resolution") == 0)
                {
                    //1920x1080
                    resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
                    PlayerPrefs.SetInt("Resolution", 0);
                    Screen.SetResolution(1920, 1080, fullscreenBoolForRes);
                    PlayerPrefs.Save();
                }
                else if (PlayerPrefs.GetInt("Resolution") == 1)
                {
                    //1366x768
                    resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
                    PlayerPrefs.SetInt("Resolution", 1);
                    Screen.SetResolution(1366, 768, fullscreenBoolForRes);
                    PlayerPrefs.Save();
                }
                else
                if (PlayerPrefs.GetInt("Resolution") == 2)
                {
                    //960x540
                    resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
                    PlayerPrefs.SetInt("Resolution", 2);
                    Screen.SetResolution(960, 540, fullscreenBoolForRes);
                    PlayerPrefs.Save();
                }
                else
                if (PlayerPrefs.GetInt("Resolution") == 3)
                {
                    //480x270
                    resolutionDropdown.value = PlayerPrefs.GetInt("Resolution");
                    PlayerPrefs.SetInt("Resolution", 3);
                    Screen.SetResolution(480, 270, fullscreenBoolForRes);
                    PlayerPrefs.Save();
                }
            }
            else
            {
                Screen.SetResolution(1920, 1080, fullscreenBoolForRes);
                PlayerPrefs.SetInt("Resolution", 0);
                PlayerPrefs.Save();
            }

        }

        //texture quality - do nothing for it

        //vsync
        if (PlayerPrefs.HasKey("VSync"))
        {
            if (PlayerPrefs.GetInt("VSync") == 0)
            {
                //turn vysnc off and make the text go off
                QualitySettings.vSyncCount = 0;
                VSyncText.text = "off";
            }
            else if (PlayerPrefs.GetInt("VSync") == 1)
            {
                //turn vsync on and make text go on
                QualitySettings.vSyncCount = 1;
                VSyncText.text = "on";
            }
        }

        //post processing
        if (PlayerPrefs.HasKey("Post Processing"))
        {
            if (PlayerPrefs.GetInt("Post Processing") == 0)
            {
                //turn Post Processing off and make the text go off
                PostProcessingText.text = "off";
            }
            else if (PlayerPrefs.GetInt("Post Processing") == 1)
            {
                //turn Post Processing on and make text go on
                PostProcessingText.text = "on";
            }
        }

        //camera fov
        if (PlayerPrefs.HasKey("FOV"))
        {
            cameraFOVSlider.value = PlayerPrefs.GetInt("FOV");
        }
        else
        {
            cameraFOVSlider.value = 60;
            PlayerPrefs.SetInt("FOV", (int) cameraFOVSlider.value);
            PlayerPrefs.Save();
        }

        //camera mode
        if (PlayerPrefs.HasKey("Camera Mode"))
        {
            cameraModeDropdown.value = PlayerPrefs.GetInt("Camera Mode");
            PlayerPrefs.SetInt("Camera Mode", cameraModeDropdown.value);
            PlayerPrefs.Save();
        }
        else
        {
            cameraModeDropdown.value = 0;
            PlayerPrefs.SetInt("Camera Mode", 0);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("Username"))
        {
            usernameOptionsPlaceholderText.text = "Change your username...";
            usernameOptionsSettingHeaderText.text = "current username:" + PlayerPrefs.GetString("Username");

        }
        else
        {
            usernameOptionsPlaceholderText.text = "Pick your username...";
        }
    }

    public void OnValueChanged()
    {
        PlayerPrefs.SetString("Username", usernameOptionsInputField.text);
        usernameOptionsSettingHeaderText.text = "current username:" + PlayerPrefs.GetString("Username");
        Launcher.instance.RefreshMenuUsername();
    }



    public void Update()
    {
        mouseText.text = "mouse look: " + MouseLook.value;
        cameraFOVText.text = "camera FOV: " + cameraFOVSlider.value;
    }

    public void SaveSensitivity()
    {
        PlayerPrefs.SetInt("Mouse", (int) MouseLook.value);
        PlayerPrefs.Save();
    }

    public void SaveFOV()
    {
        PlayerPrefs.SetInt("FOV", (int)cameraFOVSlider.value);
        PlayerPrefs.Save();
    }

    public void SaveResolution()
    {

        if (Application.platform != RuntimePlatform.WebGLPlayer)
        {

            if (resolutionDropdown.value == 0)
            {
                //1920x1080
                PlayerPrefs.SetInt("Resolution", 0);
                Screen.SetResolution(1920, 1080, fullscreenBoolForRes);
                PlayerPrefs.Save();
            }
            else if (resolutionDropdown.value == 1)
            {
                //1366x768
                PlayerPrefs.SetInt("Resolution", 1);
                Screen.SetResolution(1366, 768, fullscreenBoolForRes);
                PlayerPrefs.Save();
            }
            else
            if (resolutionDropdown.value == 2)
            {
                //960x540
                PlayerPrefs.SetInt("Resolution", 2);
                Screen.SetResolution(960, 540, fullscreenBoolForRes);
                PlayerPrefs.Save();
            }
            else
            if (resolutionDropdown.value == 3)
            {
                //480x270
                PlayerPrefs.SetInt("Resolution", 3);
                Screen.SetResolution(480, 270, fullscreenBoolForRes);
                PlayerPrefs.Save();
            }
        }
    }

    public void SaveCameraMode()
    {
        if (cameraModeDropdown.value == 0)
        {
            //third per
            PlayerPrefs.SetInt("Camera Mode", 0);
            PlayerPrefs.Save();
        }
        else if (cameraModeDropdown.value == 1)
        {
            //first per
            PlayerPrefs.SetInt("Camera Mode", 1);
            PlayerPrefs.Save();
        }
    }

    public void Fullscreen()
    {
            if (PlayerPrefs.HasKey("Fullscreen"))
            {
                if (PlayerPrefs.GetInt("Fullscreen") == 0)
                {

                    //it isnt fullscreen
                    PlayerPrefs.SetInt("Fullscreen", 1);
                    Screen.fullScreen = true;
                    FullscreenText.text = "on";
                    PlayerPrefs.Save();
                    fullscreenBoolForRes = true;
                }
                else if (PlayerPrefs.GetInt("Fullscreen") == 1)
                {
                    //it is fullscreen
                    PlayerPrefs.SetInt("Fullscreen", 0);
                    Screen.fullScreen = false;
                    FullscreenText.text = "off";
                    PlayerPrefs.Save();
                    fullscreenBoolForRes = false;
                }
            }
            else
            {
                PlayerPrefs.SetInt("Fullscreen", 1);
                Screen.fullScreen = true;
                FullscreenText.text = "on";
                PlayerPrefs.Save();
                fullscreenBoolForRes = true;
            }
    }

    public void VSync()
    {
        if (PlayerPrefs.HasKey("VSync"))
        {
            if (PlayerPrefs.GetInt("VSync") == 0)
            {
                //it isnt vsync
                QualitySettings.vSyncCount = 1;
                PlayerPrefs.SetInt("VSync", 1);
                VSyncText.text = "on";
                PlayerPrefs.Save();
            }
            else if (PlayerPrefs.GetInt("VSync") == 1)
            {
                //it is vsync
                QualitySettings.vSyncCount = 0;
                PlayerPrefs.SetInt("VSync", 0);
                VSyncText.text = "off";
                PlayerPrefs.Save();
            }
        } else
        {
            QualitySettings.vSyncCount = 1;
            PlayerPrefs.SetInt("VSync", 1);
            VSyncText.text = "on";
            PlayerPrefs.Save();
        }
    }

    public void FPSCounter()
    {
        if (PlayerPrefs.HasKey("FPS Counter"))
        {
            if (PlayerPrefs.GetInt("FPS Counter") == 0)
            {
                //it isnt shaders
                PlayerPrefs.SetInt("FPS Counter", 1);
                fpsCounterText.text = "on";
                PlayerPrefs.Save();
            }
            else if (PlayerPrefs.GetInt("FPS Counter") == 1)
            {
                //it is shaders
                PlayerPrefs.SetInt("FPS Counter", 0);
                fpsCounterText.text = "off";
                PlayerPrefs.Save();
            }
        }
        else
        {
            QualitySettings.vSyncCount = 1;
            PlayerPrefs.SetInt("FPS Counter", 1);
            fpsCounterText.text = "on";
            PlayerPrefs.Save();
        }
    }


    public void PostProcessing()
    {
        if (PlayerPrefs.HasKey("Post Processing"))
        {
            if (PlayerPrefs.GetInt("Post Processing") == 0)
            {
                //it isnt post process
                PlayerPrefs.SetInt("Post Processing", 1);
                PostProcessingText.text = "on";
                PlayerPrefs.Save();
            }
            else if (PlayerPrefs.GetInt("Post Processing") == 1)
            {
                //it is post process
                PlayerPrefs.SetInt("Post Processing", 0);
                PostProcessingText.text = "off";
                PlayerPrefs.Save();
            }
        } else
        {
            PlayerPrefs.SetInt("Post Processing", 1);
            PostProcessingText.text = "on";
            PlayerPrefs.Save();
        }
    }

    public void TextChat()
    {
        if (PlayerPrefs.HasKey("Text Chat"))
        {
            if (PlayerPrefs.GetInt("Text Chat") == 0)
            {
                //it isnt chat
                popUpExperimental.Play("WindowPopUp", 0, 0f);

            }
            else if (PlayerPrefs.GetInt("Text Chat") == 1)
            {
                //it is chat
                PlayerPrefs.SetInt("Text Chat", 0);
                textChatText.text = "off";
                PlayerPrefs.Save();
            }
        }
        else
        {
            PlayerPrefs.SetInt("Text Chat", 1);
            textChatText.text = "on";
            PlayerPrefs.Save();
        }
    }

    public void TextChatConfirmed()
    {
        //it is chat
        PlayerPrefs.SetInt("Text Chat", 1);
        textChatText.text = "on";
        PlayerPrefs.Save();
    }
}
