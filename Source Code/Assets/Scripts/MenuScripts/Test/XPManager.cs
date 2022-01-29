using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    //public GameObject XPDisplay;
    //public TMP_Text XPDisplayText;
    
    public int maxXP, playerLevel;
    public int currentXP;
    private int CachedXP;

    public TMP_Text levelText, xpCurrently;
    public static XPManager instance;

    public Image XPBar;
    bool isLoadedFirstTime;


    public void Start()
    {
        isLoadedFirstTime = false;

        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;


        playerLevel = 1;

        if (PlayerPrefs.HasKey("XP"))
        {
            currentXP = PlayerPrefs.GetInt("XP");
        } else
        {
            currentXP = 0;
            PlayerPrefs.SetInt("XP", currentXP);
            PlayerPrefs.Save();
        }





        maxXP = maxXP * playerLevel;

        isLoadedFirstTime = true;



        //xpDisplayText.text = "XP gained this Season: " + PlayerPrefs.GetInt("XP").ToString();  
    }

   

    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && isLoadedFirstTime == true)
        {
                xpCurrently.text = currentXP + " XP/" + maxXP + " XP";
                XPBar.fillAmount = currentXP / maxXP;
            levelText.text = playerLevel.ToString();

            for (int i = 0; i < CachedXP; i++)
            {
                CachedXP = CachedXP - 1;
                currentXP = currentXP + 1;
            }
            PlayerPrefs.SetInt("XP", currentXP);
            PlayerPrefs.Save();



            if (currentXP >= maxXP)
            {
                playerLevel++;
                currentXP = 0;
                maxXP += maxXP;
            }
        }
    }
    
    public void AddXP(int XPToAdd)
    {
        CachedXP = CachedXP + XPToAdd;
    }




}
