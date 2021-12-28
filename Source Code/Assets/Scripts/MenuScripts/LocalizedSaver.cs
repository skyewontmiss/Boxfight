using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SlimUI.ModernMenu;

public class LocalizedSaver : MonoBehaviour
{

    bool fullscreen, vsync, postProcessing;
    public TMP_Text volumeText, XText;
    public Slider volumeSlider, XSenseSlider;
    public OptionsMenu optionsMenu;


    void Start()
    {
        fullscreen = false;
        vsync = false;
        postProcessing = false;

        XSenseSlider.value = PlayerPrefs.GetFloat("Mouse");
        if(PlayerPrefs.HasKey("Post Processing"))
        {
            optionsMenu.CameraEffects();
        }

    }

    // Update is called once per frame
    void Update()
    {
        volumeText.text = "music volume: " + volumeSlider.value;
        XText.text = "mouse look: " + XSenseSlider.value;
    }



    public void SaveX()
    {
        PlayerPrefs.SetFloat("Mouse", XSenseSlider.value);
        PlayerPrefs.Save();
    }

    public void PostProcessing()
    {
        if(postProcessing)
        {
            PlayerPrefs.SetInt("Post Processing", 0);
            PlayerPrefs.Save();

        } else if(!postProcessing)
        {
            PlayerPrefs.SetInt("Post Processing", 1);
            PlayerPrefs.Save();
        }
    }
}
