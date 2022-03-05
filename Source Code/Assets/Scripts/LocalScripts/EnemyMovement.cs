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
    GameObject player;
    public NavMeshAgent enemy;
    public float distanceToAttack;
    public float nowdistance;
    public GameObject firstPerson, thirdPerson;
    float time;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (distanceToAttack >= nowdistance)
        {   
            nowdistance = Vector3.Distance(player.transform.position, this.gameObject.transform.position);

        } else
        {

            enemy.destination = player.transform.position;
            if(time > 4)
            {
                int random = Random.Range(4, 10);
                player.GetComponent<LocalController>().TakeDamage(15);
            }
            nowdistance = Vector3.Distance(player.transform.position, this.gameObject.transform.position);

        }
        healthFill.fillAmount = health / maxHealth;
    }



    void Start()
     {

        health = maxHealth;
        parent = transform.parent.GetComponent<WaveManager>();
        enemy.speed = speed;

        if (firstPerson == null)
        {
            player = thirdPerson;

        }
        else if (thirdPerson == null)
        {
            player = firstPerson;

        }
        else if (thirdPerson != null && firstPerson != null)
        {
            Debug.LogWarning("Too many players avaliable to track only 1.");
        }
        else 
        {
            Debug.LogWarning("No avaliable players to track.");
        }
        nowdistance = Vector3.Distance(player.transform.position, this.gameObject.transform.position);

    }

    void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Camera Mode"))
        {
            if (PlayerPrefs.GetInt("Camera Mode") == 0)
            {
                Destroy(firstPerson);

            }
            else if (PlayerPrefs.GetInt("Camera Mode") == 1)
            {
                Destroy(thirdPerson);
            }

        }
        else
        {
            Destroy(firstPerson);
        }
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
