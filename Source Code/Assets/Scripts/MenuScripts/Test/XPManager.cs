using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class XPManager : MonoBehaviour
{
    //public GameObject XPDisplay;
    //public TMP_Text XPDisplayText;

    public int maxXP, playerLevel;
    public float updatedXP;

    public TMP_Text levelText, xpCurrently;

    public Image XPBar;

    //for test
    public float XPIncreasedPerSecond;


    public void Start()
    {
        playerLevel = 1;
        XPIncreasedPerSecond = 5f;
        maxXP = 25;
        updatedXP = 0;







        //xpDisplayText.text = "XP gained this Season: " + PlayerPrefs.GetInt("XP").ToString();  
    }

    public void Update()
    {
        updatedXP += XPIncreasedPerSecond;
        XPBar.fillAmount = updatedXP / maxXP;

        levelText.text = playerLevel.ToString();
        xpCurrently.text = updatedXP + " XP/" + maxXP + " XP"; 

        if(updatedXP >= maxXP)
        {
            playerLevel++;
            updatedXP = 0;
            maxXP += maxXP;
        }








    //    if(Input.GetKey(KeyCode.LeftShift))
    //    {
    //        if(Input.GetKey(KeyCode.X))
    //        {
    //            if(Input.GetKey(KeyCode.P))
    //            {
    //                XPDisplay.SetActive(true);
    //            }
    //        }
    //    } else
    //    {
    //        XPDisplay.SetActive(false);
    //    }
    }
}
