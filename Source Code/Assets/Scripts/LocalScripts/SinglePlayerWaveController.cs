using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayerWaveController : MonoBehaviour
{
    public GameObject[] Waves;
    public int waveIndex;
    public static SinglePlayerWaveController instance;
    

    void Start()
    {
        waveIndex = -1;
        instance = this;
        RefreshWaves();
        NextWave();
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
            AchievementManager.instance.AchievementGet("Slayer");
        }
        RefreshWaves();
        waveIndex += 1;
        Waves[waveIndex].SetActive(true);
    }


}
