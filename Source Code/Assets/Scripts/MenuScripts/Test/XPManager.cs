using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    public GameObject XPDisplay;
    public TMP_Text xpDisplayText;

    public void Start()
    {
        xpDisplayText.text = "XP gained this Season: " + PlayerPrefs.GetInt("XP").ToString();  
    }

    public void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(Input.GetKey(KeyCode.X))
            {
                if(Input.GetKey(KeyCode.P))
                {
                    XPDisplay.SetActive(true);
                }
            }
        } else
        {
            XPDisplay.SetActive(false);
        }
    }
}
