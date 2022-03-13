using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SinglePlayerWaveController : MonoBehaviour
{
    [Header("SPWC.WaveManager")]
    public GameObject[] Waves;
    public int waveIndex;
    public int numberOfWaves;
    public static SinglePlayerWaveController instance;

    [Header("SPWC.StoryBoardManager")]
    public Animator storyAnimator;
    public TMP_Text StoryboardText;
    public string[] Storyboard;
    public string[] endOfWorldStoryboard;

    [Header("SPWC.PlayerManager")]
    public GameObject firstPerson, thirdPerson;
    GameObject player;

    [Header("SPWC.SceneLoading")]
    public string LevelToLoad;
    public bool isEndOfWorld;
    

    void Start()
    {
        if (PlayerPrefs.HasKey("Camera Mode"))
        {
            if (PlayerPrefs.GetInt("Camera Mode") == 0)
            {
                Destroy(firstPerson);
                firstPerson = null;
                player = thirdPerson;
            }
            else if (PlayerPrefs.GetInt("Camera Mode") == 1)
            {
                Destroy(thirdPerson);
                thirdPerson = null;
                player = firstPerson;
            }

        } else
        {
            Destroy(firstPerson);
            firstPerson = null;
            player = thirdPerson;
        }
        waveIndex = -1;
        instance = this;
        RefreshWaves();
        storyAnimator.Play("Popup", 0, 0f);
        StoryboardText.text = "Hey!";
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
                AchievementManager.instance.AchievementGet("Box Slayer");
            }
        }
        waveIndex += 1;
        RefreshWaves();

        if(waveIndex >= numberOfWaves)
        {
            if(isEndOfWorld)
            {
                storyAnimator.Play("Popup", 0, 0f);
                StoryboardText.text = "You finished them all!";
                StartCoroutine(EndOfWorldStory());
                return;

            } else if(!isEndOfWorld)
            {
                player.GetComponent<LocalController>().LoadNextLevel(LevelToLoad);
                return;
            }
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

        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));

        foreach (string Str in Storyboard)
        {
            yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
            storyAnimator.Play("Popup", 0, 0f);
            StoryboardText.text = Str;
            yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));

        }
        NextWave();
        storyAnimator.Play("Idle", 0, 0f);


    }

    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode))
            yield return null;
    }

    IEnumerator EndOfWorldStory()
    {

        yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));

        foreach (string Str in endOfWorldStoryboard)
        {
            yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));
            storyAnimator.Play("Popup", 0, 0f);
            StoryboardText.text = Str;
            yield return StartCoroutine(WaitForKeyDown(KeyCode.Return));

        }
        player.GetComponent<LocalController>().LoadNextLevel("Menu Scene");

        storyAnimator.Play("Idle", 0, 0f);


    }


    #endregion



}
