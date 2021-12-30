using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    }

    // Update is called once per frame
    public void AchievementGet(string achievementName)
    {
        StartCoroutine(AchievementAnimate("Slayer"));
    }

    IEnumerator AchievementAnimate (string achievementName)
    {
        if (achievementName == "Slayer")
        {
            achievementNameText.text = "Slayer";
            achievementDescriptionText.text = "Complete a full wave of the Single Player Campaign. No sweat.";
            myself.SetActive(true);
            yield return new WaitForSeconds(4f);
            myself.GetComponent<Animator>().Play("achievementEscape", 0, 0f);
            yield return new WaitForSeconds(1.75f);
            myself.SetActive(false);

        }
    }

}
