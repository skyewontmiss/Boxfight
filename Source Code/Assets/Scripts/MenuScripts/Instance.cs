using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instance : MonoBehaviour
{
    public static Instance instance;
    public string sceneToDestroyWhenLoaded;

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

    public void Update()
    {
        if(SceneManager.GetActiveScene().name == sceneToDestroyWhenLoaded)
        {
            Destroy(this.gameObject);
        }
    }
}
