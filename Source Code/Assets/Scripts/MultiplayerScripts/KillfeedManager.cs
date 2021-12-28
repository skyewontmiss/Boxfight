using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillfeedManager : MonoBehaviour
{
    public static KillfeedManager instance;
    

    void Awake()
    {
        instance = this;
    }


}
