using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
     public Transform Player;
     public float RandomSpeedBase = 4;
     public float MaxDistanceToStopMovingTowardPlayer = 10;
     public float MinDistanceToMoveTowardPlayer = 5;
    private int speed;
    private WaveManager parent;
 
 
 
 
     void Start()
     {
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
     }


    public void IWillDieable()
    {
        parent.RemoveEnemy();
        Destroy(this.gameObject);
    }
} 
