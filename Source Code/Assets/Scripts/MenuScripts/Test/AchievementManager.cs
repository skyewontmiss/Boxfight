using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    public TMP_Text achievementNameText, achievementDescriptionText;
    public GameObject myself;
    float timer;

    void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    // Update is called once per frame
    public void AchievementGet(string achievementName)
    {
        timer = 0;
        if(achievementName == "Slayer")
        {
            achievementNameText.text = "Slayer";
            achievementDescriptionText.text = "Complete a full wave of the Single Player Campaign. No sweat.";
            myself.SetActive(true);

        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            myself.SetActive(false);
        }
    }
}
