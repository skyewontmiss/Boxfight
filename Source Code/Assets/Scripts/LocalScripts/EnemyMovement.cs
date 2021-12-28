using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
     public Transform Player;
     public float RandomSpeedBase = 4;
     public float MaxDistanceToStopMovingTowardPlayer = 10;
     public float MinDistanceToMoveTowardPlayer = 5;
    private int speed;
    private WaveManager parent;
    public float maxHealth;
    float health;
    public Image healthFill;
 
 
 
 
     void Start()
     {
        health = maxHealth;
        speed = (int)Random.Range(RandomSpeedBase - 2, RandomSpeedBase + 5);
        parent = transform.parent.GetComponent<WaveManager>();
 
     }
 
     void Update()
     {
         transform.LookAt(Player);
 
         if (Vector3.Distance(transform.position, Player.position) >= MinDistanceToMoveTowardPlayer)        
        {

            transform.position += transform.forward * speed * Time.deltaTime;
 
 
 
             if (Vector3.Distance(transform.position, Player.position) <= MaxDistanceToStopMovingTowardPlayer)
             {
                 //Here Call any function U want Like Shoot at here or something
             }
 
         }

        healthFill.fillAmount = health / maxHealth;
    }


    public void IWillDieable(float damageToTake)
    {
        if(health > 0)
        {
            health = health - damageToTake;
            if(health <= 0)
            {
                parent.RemoveEnemy();
                Destroy(this.gameObject);
            }

        } else
        {
            parent.RemoveEnemy();
            Destroy(this.gameObject);
        }
    }
} 
