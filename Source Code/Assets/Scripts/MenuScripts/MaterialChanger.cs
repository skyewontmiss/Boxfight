using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
{
    // Start is called before the first frame update

    public Material[] possibleSkins;

    void Start()
    {
        this.gameObject.GetComponent<MeshRenderer>().material = possibleSkins[Random.Range(0, possibleSkins.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
