using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waffle : MonoBehaviour
{

    public int minimumHealth, maximumHealth;
    [Header("Multiplayer")]
    public PhotonView PV;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Game Player"))
        {
            PlayerController player = other.gameObject.GetComponent<PlayerController>();
            if(player != null)
            {
                float healthToGive = Random.Range(minimumHealth, maximumHealth);
                player.GainHealth(healthToGive);
                
            }
        }

        if(PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}
