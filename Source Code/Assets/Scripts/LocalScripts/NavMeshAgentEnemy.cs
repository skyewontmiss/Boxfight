using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentEnemy : MonoBehaviour
{
    //add NavMeshAgent component to the enemy and bake the mesh so it can follow you
    //easy enemy 
    public GameObject player;
    private NavMeshAgent enemy;
    public float distanceToAtack;
    public float nowdistance;
    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy.destination = player.transform.position;
        nowdistance = Vector3.Distance(player.transform.position, this.gameObject.transform.position);
        if(distanceToAtack >= nowdistance)
        {
            Debug.Log("TOO CLOSE ATACK");
        }
    }
}
