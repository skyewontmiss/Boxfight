using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyMovement : MonoBehaviour
{
    public int speed = 2;
    private WaveManager parent;
    public float maxHealth;
    float health;
    public Image healthFill;
    public GameObject player;
    public NavMeshAgent enemy;
    public float distanceToAttack;
    public float nowdistance;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (distanceToAttack >= nowdistance)
        {   
            Debug.Log("TOO CLOSE ATACK");

        } else
        {
            //makes sure NavMeshAgent doesn't go past the nowDistance if we are too close to the player
            enemy.destination = player.transform.position;
            nowdistance = Vector3.Distance(player.transform.position, this.gameObject.transform.position);

        }
        healthFill.fillAmount = health / maxHealth;
    }



    void Start()
     {
        health = maxHealth;
        parent = transform.parent.GetComponent<WaveManager>();
        nowdistance = Vector3.Distance(player.transform.position, this.gameObject.transform.position);

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
