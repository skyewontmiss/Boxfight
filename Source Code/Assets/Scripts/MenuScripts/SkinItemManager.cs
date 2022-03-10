using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkinItemManager : MonoBehaviour
{
    SkinItem[] skinItems;
    public TMP_Text itemNameText, itemDescriptionText;
    public int ItemIDRN;
    public GameObject applyButton;

    void Start()
    {
        GameObject[] LockerItemGameObjects = GameObject.FindGameObjectsWithTag("Locker Item");
        skinItems = new SkinItem[LockerItemGameObjects.Length];
        for (int i = 0; i < LockerItemGameObjects.Length; ++i)
        {
            skinItems[i] = LockerItemGameObjects[i].GetComponent<SkinItem>();
        }

        itemNameText.text = "";
        itemDescriptionText.text = "";
        RefreshItems();
    }

    void RefreshItems()
    {
        int itemID = 0;
        //initialising each skin by updating the visuals of every item in the locker
        foreach (SkinItem skinItem in skinItems)
        {
            skinItem.ItemID = itemID;
            Image imageVisuals = skinItem.gameObject.transform.Find("ItemVisuals").GetComponent<Image>();
            imageVisuals.sprite = skinItem.skinSprite;
            itemID += 1;
        }
    }

    public void OnItemClicked(int itemID)
    {
        //parsing in variables for things.
        for (int i = 0; i < skinItems.Length; ++i)
        {
            if (skinItems[i].ItemID == itemID)
            {
                itemNameText.text = skinItems[i].ItemName;
                if(skinItems[i].isDLCItem)
                {
                    itemNameText.text = itemNameText.text + " | <color=#FF3087>I<color=#34CEF7>X <color=#30F3DC>DLC</color> <color=#FFE800>Exclusive </color>";

                } else
                {
                    itemNameText.text = itemNameText.text;
                }

                itemDescriptionText.text = skinItems[i].ItemDescription;
                break;
            }
        }
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
