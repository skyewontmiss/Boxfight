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
    public bool isDLCItem;


    private void Awake()
    {
        ItemName = EditableItemName;
        ItemDescription = EditableItemDescription;


        if(PlayerPrefs.HasKey("Skin"))
        {
            if (ItemID == PlayerPrefs.GetInt("Skin"))
            {
                GameObject displayCube = GameObject.FindGameObjectWithTag("Display Cube");
                displayCube.GetComponent<MeshRenderer>().material = skinMaterial;
                displayCube.GetComponent<DisplayRotator>().CreateUV();
                
            }
        }

    }

    public void OnClick()
    {
        GameObject displayCube = GameObject.FindGameObjectWithTag("Display Cube");
        displayCube.GetComponent<MeshRenderer>().material = skinMaterial;
        displayCube.GetComponent<DisplayRotator>().CreateUV();
        SkinItemManager Manager = GameObject.FindGameObjectWithTag("Skin Item Manager").GetComponent<SkinItemManager>();
        Manager.OnItemClicked(ItemID);

        if(isDLCItem)
        {
            //add authentication stuff here

            Manager.applyButton.SetActive(true);

        } else
        {
            Manager.applyButton.SetActive(false);

            PlayerPrefs.SetInt("Skin", ItemID);
            PlayerPrefs.Save();
        }

    }
}
