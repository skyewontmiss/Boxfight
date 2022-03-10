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

     bool isLoadedFirstTime;
    public Animator LEVELUPOBJECT, levelImageAnimator;
    public GameObject[] GOs;
    RectTransform transformR;
    public Image level;

    public Sprite[] levelImages;

    



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
        RefreshLevel();







        isLoadedFirstTime = true;




    }

    public  void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public  void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {

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
                RefreshLevel();
                PlayerPrefs.SetInt("XP", currentXP);
                PlayerPrefs.Save();
            }

            xpCurrently.text = currentXP + " XP/" + maxXP + " XP";
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

    public void RefreshLevel()
    {
        StartCoroutine(Animate());
    }

    IEnumerator Animate()
    {
        if (playerLevel > 0 && playerLevel < 5)
        {
            levelImageAnimator.Play("nextLevel");
            yield return new WaitForSeconds(0.30f);
            level.sprite = levelImages[0];
        }
        else if (playerLevel > 4 && playerLevel < 10)
        {
            levelImageAnimator.Play("nextLevel");
            yield return new WaitForSeconds(0.30f);
            level.sprite = levelImages[1];
        }
        else if (playerLevel > 9 && playerLevel < 15)
        {
            levelImageAnimator.Play("nextLevel");
            yield return new WaitForSeconds(0.30f);
            level.sprite = levelImages[2];
        }
        else if (playerLevel > 14 && playerLevel < 20)
        {
            levelImageAnimator.Play("nextLevel");
            yield return new WaitForSeconds(0.30f);
            level.sprite = levelImages[3];
        }
        else if (playerLevel > 19 && playerLevel < 25)
        {
            levelImageAnimator.Play("nextLevel");
            yield return new WaitForSeconds(0.30f);
            level.sprite = levelImages[4];
        }
        else if (playerLevel > 24 && playerLevel < 30)
        {
            levelImageAnimator.Play("nextLevel");
            yield return new WaitForSeconds(0.30f);
            level.sprite = levelImages[5];
        }
        else if (playerLevel > 29 && playerLevel < 35)
        {
            levelImageAnimator.Play("nextLevel");
            yield return new WaitForSeconds(0.30f);
            level.sprite = levelImages[6];
        }
    }




}
