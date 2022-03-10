using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    // called zero
    public static TransitionManager instance;
    public Animator animator;

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

    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void Close()
    {
        animator.Play("Close");
    }




    // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        animator.Play("Open");
    }

    // called third
    void Start()
    {

    }

    // called when the game is terminated
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}

