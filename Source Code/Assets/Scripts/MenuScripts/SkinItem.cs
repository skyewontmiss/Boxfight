using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinItem : MonoBehaviour
{
    public string EditableItemName;
    public string EditableItemDescription;
    public Sprite skinSprite;
    public Material skinMaterial;
    public int ItemID;
    [HideInInspector] public string ItemName;
    [HideInInspector] public string ItemDescription;


    private void Awake()
    {
        ItemName = EditableItemName;
        ItemDescription = EditableItemDescription;
    }

    public void OnClick()
    {
        GameObject displayCube = GameObject.FindGameObjectWithTag("Display Cube");
        displayCube.GetComponent<MeshRenderer>().material = skinMaterial;
        displayCube.GetComponent<DisplayRotator>().CreateUV();
        GameObject.FindGameObjectWithTag("Skin Item Manager").GetComponent<SkinItemManager>().OnItemClicked(ItemID);
        PlayerPrefs.SetInt("Skin", ItemID);
        PlayerPrefs.Save();
    }
}
