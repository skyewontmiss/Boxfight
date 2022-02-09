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

        } else
        {
            Destroy(firstPerson);
            firstPerson = null;
        }
        waveIndex = -1;
        instance = this;
        RefreshWaves();
        StartCoroutine(Storyboard1());

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
        if (waveIndex == 0)
        {
            if(AchievementManager.instance != null)
            {
                AchievementManager.instance.AchievementGet("Slayer");
            }
        }
        RefreshWaves();
        waveIndex += 1;
        if(Waves[waveIndex] == null)
        {
            Debug.Log("End");
        }
        StartCoroutine(WaveAnnounce());
        
        Waves[waveIndex].SetActive(true);
    }

    IEnumerator WaveAnnounce()
    {
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "Starting wave " + waveIndex + "...";
        yield return new WaitForSeconds(4f);
        storyAnimator.Play("Idle", 0, 0f);
    }

    #region level1

    IEnumerator Storyboard1()
    {
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "We've been expecting you.";
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "Anyway, let's explain what your doing here.";
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "You gotta defeat all the enemies in each wave.";
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "There are 7 waves in each level.";
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "You have to clear all the enemies in each level.";
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "Alright, from here on out, your gonna have to go without me.";
        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
        NextWave();
        storyAnimator.Play("Idle", 0, 0f);

    }

    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }

    #endregion



}
