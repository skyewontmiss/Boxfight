using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class ModLoader : MonoBehaviour
{

    public string Path;
    MonoBehaviour unityScript;

    void Start()
    {
        Assembly thing = Assembly.LoadFile(Path);
        Debug.Log(thing.ToString());

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
