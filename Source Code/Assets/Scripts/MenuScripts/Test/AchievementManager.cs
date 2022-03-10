using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public TMP_Text achievementNameText, achievementDescriptionText;
    public GameObject myself;
    public int MaxAchievements;
    int currentAchievements;

    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
        myself.SetActive(false);


    }

    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            ImportAssets();
        }
    }


    void ImportAssets()
    {
        currentAchievements = 0;
        string FilePath = Application.persistentDataPath + "/Achievements.txt";
        string[] lines;
        if (File.Exists(FilePath))
        {
            lines = File.ReadAllLines(FilePath);

        }
        else
        {
            File.Create(FilePath);
            lines = File.ReadAllLines(FilePath);
        }

        GameObject[] AchievementItems = GameObject.FindGameObjectsWithTag("Achievement Item");

        foreach (GameObject achievement in AchievementItems)
        {
            Image image = achievement.GetComponent<Image>();
            

            foreach (string line in lines)
            {
                if (line == achievement.name)
                {
                    //ends all the code
                    image.color = new Color(0, 255, 0);
                    currentAchievements = currentAchievements + 1;
                } else
                {
                    
                }
            }



        }

        GameObject completionObject = GameObject.Find("Completion");

        completionObject.GetComponent<TMP_Text>().text = currentAchievements + "/" + MaxAchievements + " Achievements completed!";
    }

    // Update is called once per frame
    public void AchievementGet(string achievementName)
    {
        StartCoroutine(AchievementAnimate("Box Slayer"));
    }

    IEnumerator AchievementAnimate (string achievementName)
    {
        string FilePath = Application.persistentDataPath + "/Achievements.txt";
        string[] lines;

        if(File.Exists(FilePath))
        {
            lines = File.ReadAllLines(FilePath);

        } else
        {
            File.Create(FilePath);
            lines = File.ReadAllLines(FilePath);
        }

        if (achievementName == "Box Slayer")
        {
            foreach (string line in lines)
            {
                if(line == "Box Slayer")
                {
                    //ends all the code
                    yield break;
                }
            }
            File.AppendAllText(FilePath, "Box Slayer");
            achievementNameText.text = "Box Slayer";
            achievementDescriptionText.text = "Complete a full wave of the Single Player Campaign. No sweat.";
            myself.SetActive(true);
            yield return new WaitForSeconds(4f);
            myself.GetComponent<Animator>().Play("achievementEscape", 0, 0f);
            yield return new WaitForSeconds(1.75f);
            myself.SetActive(false);

        }
        else if (achievementName == "The First")
        {
            foreach (string line in lines)
            {
                if (line == "The First")
                {
                    //ends all the code
                    yield break;
                }
            }
            File.AppendAllText(FilePath, "The First");
            achievementNameText.text = "The First";
            achievementDescriptionText.text = "Join into your first multiplayer lobby. Welcome to Boxfight!";
            myself.SetActive(true);
            yield return new WaitForSeconds(4f);
            myself.GetComponent<Animator>().Play("achievementEscape", 0, 0f);
            yield return new WaitForSeconds(1.75f);
            myself.SetActive(false);

        } else if(achievementName == "Skill Issue")
        {
            foreach (string line in lines)
            {
                if (line == "Skill Issue")
                {
                    //ends all the code
                    yield break;
                }
            }
            File.AppendAllText(FilePath, "Skill Issue");
            achievementNameText.text = "Skill Issue";
            achievementDescriptionText.text = "Die from a player for the first time in multiplayer. Your terrible at this.";
            myself.SetActive(true);
            yield return new WaitForSeconds(4f);
            myself.GetComponent<Animator>().Play("achievementEscape", 0, 0f);
            yield return new WaitForSeconds(1.75f);
            myself.SetActive(false);

        } else if(achievementName == "Rule the World...")
        {
            foreach (string line in lines)
            {
                if (line == "Rule the World...")
                {
                    //ends all the code
                    yield break;
                }
            }
            File.AppendAllText(FilePath, "Rule the World...");
            achievementNameText.text = "Rule the World...";
            achievementDescriptionText.text = "...maybe not. Your first by falling out of the world? Lol. Newb.";
            myself.SetActive(true);
            yield return new WaitForSeconds(4f);
            myself.GetComponent<Animator>().Play("achievementEscape", 0, 0f);
            yield return new WaitForSeconds(1.75f);
            myself.SetActive(false);

        } else if (achievementName == "Rifle Enthusiast")
        {
            foreach (string line in lines)
            {
                if (line == "Rifle Enthusiast")
                {
                    //ends all the code
                    yield break;
                }
            }
            File.AppendAllText(FilePath, "Rifle Enthusiast");
            achievementNameText.text = "Rifle Enthusiast";
            achievementDescriptionText.text = "Shoot your Assault Rifle 900 times.";
            myself.SetActive(true);
            yield return new WaitForSeconds(4f);
            myself.GetComponent<Animator>().Play("achievementEscape", 0, 0f);
            yield return new WaitForSeconds(1.75f);
            myself.SetActive(false);
        }
    }

 

}
