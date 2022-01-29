using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public TMP_Text achievementNameText, achievementDescriptionText;
    public GameObject myself;

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

        if(SceneManager.GetActiveScene().buildIndex == 0)
        {
            ImportAssets();
        }
    }

    void ImportAssets()
    {
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
                    image.color = new Color(18, 255, 0);
                } else
                {
                    
                }
            }



        }
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

        }
    }

}
