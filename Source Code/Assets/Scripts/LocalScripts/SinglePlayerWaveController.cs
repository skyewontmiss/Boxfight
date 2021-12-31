using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SinglePlayerWaveController : MonoBehaviour
{
    public GameObject[] Waves;
    public int waveIndex;
    public static SinglePlayerWaveController instance;
    public Animator storyAnimator;
    public TMP_Text StoryboardText;
    public GameObject firstPerson, thirdPerson;
    

    void Start()
    {
        if (PlayerPrefs.HasKey("Camera Mode"))
        {
            if (PlayerPrefs.GetInt("Camera Mode") == 0)
            {
                Destroy(firstPerson);
                firstPerson = null;
            }
            else if (PlayerPrefs.GetInt("Camera Mode") == 1)
            {
                Destroy(thirdPerson);
                thirdPerson = null;
            }

        }
        waveIndex = -1;
        instance = this;
        RefreshWaves();
        StartCoroutine(StoryboardStart());

    }

    void RefreshWaves()
    {
        foreach (GameObject wave in Waves)
        {
            wave.SetActive(false);
        }
    }

    public void NextWave()
    {
        if(waveIndex == 0)
        {
            if(AchievementManager.instance != null)
            {
                AchievementManager.instance.AchievementGet("Slayer");
            }
        }
        RefreshWaves();
        waveIndex += 1;
        if(waveIndex == 1)
        {
            //StartCoroutine(StoryboardElement("Huh. Looks like you can take it.", 6f));
            //StartCoroutine(StoryboardElement("But we aren't making this easy.", 6f));
            //StartCoroutine(StoryboardElement("Let's do this again.", 4f));
        }
        Waves[waveIndex].SetActive(true);
    }

    IEnumerator StoryboardStart()
    {
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "We've been expecting you.";
        yield return new WaitForSeconds(3f);
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "You gotta defeat all the enemies.";
        yield return new WaitForSeconds(5f);
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "Anyway, let's explain what your doing here.";
        yield return new WaitForSeconds(5f);
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "You gotta defeat all the enemies.";
        yield return new WaitForSeconds(4f);
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "If you don't, they kill you. Or we kill you.";
        yield return new WaitForSeconds(6f);
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "Let's see how you do with the first wave.";
        yield return new WaitForSeconds(6f);
        NextWave();

    }



}
