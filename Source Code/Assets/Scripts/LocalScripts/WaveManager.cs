using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    GameObject[] enemysInMyWave;
    int enemies;

    private void Awake()
    {
        enemysInMyWave = GameObject.FindGameObjectsWithTag("Single Player Enemy");
        enemies = enemysInMyWave.Length;
    }

    public void RemoveEnemy()
    {
        if(enemies > 1)
        {
            enemies = enemies - 1;
            if(enemies == 0)
            {
                Debug.Log("End of wave!");
                SinglePlayerWaveController.instance.NextWave();
            }
        } else
        {
            Debug.Log("End of wave!");
            SinglePlayerWaveController.instance.NextWave();
        }
    }


}
