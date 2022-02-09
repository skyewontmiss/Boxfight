using Photon.Pun;
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
    private int currentXP;
    private int CachedXP;

    public TMP_Text levelText, xpCurrently, levelUpText;
    public static XPManager instance;

    public Image XPBar;
    bool isLoadedFirstTime;
    public Animator LEVELUPOBJECT;
    public GameObject[] GOs;
    RectTransform transformR;
    float percent;

    



    public void Start()
    {



        foreach (GameObject objectG in GOs)
        {
            objectG.SetActive(false);
        }

        isLoadedFirstTime = false;

        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;




        if (PlayerPrefs.HasKey("Level"))
        {
            playerLevel = PlayerPrefs.GetInt("Level");

        }
        else
        {
            playerLevel = 1;
            PlayerPrefs.SetInt("Level", playerLevel);
            PlayerPrefs.Save();
        }

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
        percent = currentXP / maxXP;







        isLoadedFirstTime = true;




    }

    public  void OnEnable()
    {
        Debug.Log("OnEnableCalled");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public  void OnDisable()
    {
        Debug.Log("OnDisableCalled");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log("OnSceneLoadedCalled");
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            transformR = GameObject.FindGameObjectWithTag("MenuMain").GetComponent<RectTransform>();
        }

    }



    public void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && isLoadedFirstTime == true)
        {

  


            foreach (GameObject objectG in GOs)
            {
                objectG.SetActive(true);
            }


            if(transformR != null)
            {
                if (transformR.localScale.x < 0.99)
                {
                    if (transformR.localScale.y < 0.99)
                    {
                        if (transformR.localScale.z < 0.99)
                        {
                            foreach (GameObject objectG in GOs)
                            {
                                objectG.SetActive(false);
                            }
                        }
                    }
                }
            } else
            {
                Debug.LogWarning("No suitable menu object to check for XP.");
            }



            if(CachedXP > 0)
            {
                for (int i = 0; i < CachedXP; i++)
                {
                    CachedXP = CachedXP - 1;
                    currentXP = currentXP + 1;
                }
                PlayerPrefs.SetInt("XP", currentXP);
                PlayerPrefs.Save();
            }






            if (currentXP >= maxXP)
            {
                LEVELUPOBJECT.Play("levelUpIn");
                playerLevel++;
                PlayerPrefs.SetInt("Level", playerLevel);
                PlayerPrefs.Save();
                levelUpText.text = "Level " + playerLevel + " Reached!";
                currentXP = currentXP - maxXP;
                maxXP += maxXP;
                PlayerPrefs.SetInt("XP", currentXP);
                PlayerPrefs.Save();
            }

            Debug.Log("Percent " + percent);
            percent = currentXP / maxXP;
            xpCurrently.text = currentXP + " XP/" + maxXP + " XP";
            XPBar.fillAmount = percent;
            levelText.text = playerLevel.ToString();


        } else
        {
            foreach (GameObject objectG in GOs)
            {
                objectG.SetActive(false);
            }
        }
    }
    
    public void AddXP(int XPToAdd)
    {
        CachedXP = CachedXP + XPToAdd;
    }




}
