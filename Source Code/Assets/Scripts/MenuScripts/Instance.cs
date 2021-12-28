using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instance : MonoBehaviour
{
    public static Instance instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void OnDestroy()
    {
        Destroy(this.gameObject);
    }
}
