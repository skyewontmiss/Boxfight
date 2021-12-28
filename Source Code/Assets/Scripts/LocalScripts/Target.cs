using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

public AudioSource sound;
public AudioClip[] hit;

    

    public void ChangePos()
    {
        //create boundary
        transform.position = TargetBounds.instance.GetRandomPosition();
         sound.clip = hit[Random.Range(0, hit.Length)];
         sound.Play ();
    }

}
