using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEditor;

public class CustomStuff : MonoBehaviour
{
    //for customBackgrounds
    public GameObject backgroundButton;
    public GameObject CustomBackgroundsButtonHolder;
    public Image customBackground;

    //for custom snow particles
    public GameObject snowButton;
    public GameObject CustomSnowParticlesButtonHolder;
    public Material customSnowMaterial;



    //for other thingies. 
    public GameObject setup, setupSpecific;
    public GameObject[] customMenus;
    // Start is called before the first frame update

    string inputFieldPath;
    string imagePath;
    FileInfo[] info;
    public Texture AtStart;
    public Material SnowMaterial;

    private void Start()
    {
        SnowMaterial.mainTexture = AtStart;


        if (PlayerPrefs.HasKey("Custom Path"))
        {
            if(Directory.Exists(PlayerPrefs.GetString("Custom Path")))
            {
                inputFieldPath = PlayerPrefs.GetString("Custom Path");
            }

        } else
        {
            //end function

        }
        foreach (GameObject customMenu in customMenus)
        {
            customMenu.SetActive(false);
        }

        Debug.Log(inputFieldPath);
        RefreshMenus();
    } 

    public void UpdatePath(TMP_InputField inputField)
    {
        PlayerPrefs.SetString("Custom Path", inputField.text);
    }

    public void Setup()
    {
        Directory.CreateDirectory(inputFieldPath + "/Custom Stuff");
        RefreshMenus();
    }

    public void SetupSpecific(string WhatToSetUp)
    {
        Directory.CreateDirectory(inputFieldPath + "/Custom Stuff/"  + WhatToSetUp);
        RefreshMenus();
        foreach (GameObject customMenu in customMenus)
        {
            customMenu.SetActive(false);
        }
        setupSpecific.SetActive(false);
        setup.SetActive(false);

        if(WhatToSetUp == "Custom Menu Backgrounds")
        {
            customMenus[0].SetActive(true);

        } else if(WhatToSetUp == "Custom Snow Particles")
        {
            customMenus[1].SetActive(true);

        } else
        {
            Debug.LogError("Can't find the folder you're looking for! Please check the name of the path your creating and try again.");
        }

        RefreshCustomBackgrounds();
        RefreshCustomSnow();
    }


    public void BackButton()
    {
        foreach (GameObject customMenu in customMenus)
        {
            customMenu.SetActive(false);
        }
        RefreshMenus();
        setupSpecific.SetActive(true);
        setup.SetActive(false);
    }

    public void CopyFolder (string WhatToCopy)
    {
        CopyToClipboard(inputFieldPath + "/Custom Stuff/" + WhatToCopy);
    }

    void CopyToClipboard(string clipboard)
    {
        GUIUtility.systemCopyBuffer = clipboard;
    }

    public void RefreshMenus()
    {
        if(Directory.Exists(inputFieldPath + "/Custom Stuff"))
        {

            if(Directory.Exists(inputFieldPath + "/Custom Stuff/Custom Menu Backgrounds") || Directory.Exists(inputFieldPath + "/Custom Stuff/Custom Snow Particles") || Directory.Exists(inputFieldPath + "/Custom Stuff/Custom Menu Backgrounds") && Directory.Exists(inputFieldPath + "/Custom Stuff/Custom Snow Particles"))
            {
                setupSpecific.SetActive(true);
                setup.SetActive(false);
            } else
            {
                setupSpecific.SetActive(true);
                setup.SetActive(false);
            }

        } else
        { 
            setupSpecific.SetActive(false);
            setup.SetActive(true);
        }
    }


    public void RefreshCustomBackgrounds()
    {
        foreach (RectTransform child in CustomBackgroundsButtonHolder.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        imagePath = inputFieldPath + "/Custom Stuff/Custom Menu Backgrounds";
        DirectoryInfo dir = new DirectoryInfo(imagePath);
        info = dir.GetFiles("*.*");

        foreach (FileInfo f in info)
        {
            string file = f.ToString();
            //checks if the file is not a unity generated file
            if (!file.Contains(".meta"))
            {
                //checks if the file is a compatible file (which in our case is a .png)
                if (file.Contains(".png"))
                {
                    GameObject customBackgroundButton = Instantiate(backgroundButton, CustomBackgroundsButtonHolder.transform);
                    customBackgroundButton.GetComponentInChildren<TMP_Text>().text = f.Name;
                }
            }
        }
    }

        public void RefreshCustomSnow()
        {
            foreach (RectTransform child in CustomSnowParticlesButtonHolder.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            imagePath = inputFieldPath + "/Custom Stuff/Custom Snow Particles";
        DirectoryInfo dir = new DirectoryInfo(imagePath);
            info = dir.GetFiles("*.*");

            foreach (FileInfo f in info)
            {
                string file = f.ToString();
                //checks if the file is not a unity generated file
                if (!file.Contains(".meta"))
                {
                    //checks if the file is a compatible file (which in our case is a .png)
                    if (file.Contains(".png"))
                    {
                        GameObject CustomSnowParticlesButton = Instantiate(snowButton, CustomSnowParticlesButtonHolder.transform);
                    CustomSnowParticlesButton.GetComponentInChildren<TMP_Text>().text = f.Name;
                    }
                }
            }

        }
}
