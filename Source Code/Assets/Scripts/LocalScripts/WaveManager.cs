using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    EnemyMovement[] enemysInMyWave;
    public int enemies;

    private void Awake()
    {
        enemysInMyWave = this.gameObject.GetComponentsInChildren<EnemyMovement>();
        enemies = enemysInMyWave.Length;
    }

    public void RemoveEnemy()
    {
        if(enemies > 0)
        {
            enemies = enemies - 1;

            if (enemies == 0)
            {
                Debug.Log("End of wave!");
                SinglePlayerWaveController.instance.NextWave();
            }



            if (XPManager.instance != null)
            {
                XPManager.instance.AddXP(30);
            }



        }
        else if (enemies == 0)
        {
            Debug.Log("End of wave!");
            SinglePlayerWaveController.instance.NextWave();

        } else
        {

        }
    }


}
