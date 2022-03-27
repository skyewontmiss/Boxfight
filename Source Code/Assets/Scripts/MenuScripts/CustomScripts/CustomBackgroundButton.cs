using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CustomBackgroundButton : MonoBehaviour
{
    GameObject customBackgroundObject;
    public TMP_Text text;
    public Material snowMaterial;

    private void Start()
    {
        customBackgroundObject = GameObject.FindGameObjectWithTag("Custom Background");
    }
    public void ImageProcess()
    {
        string imageName = text.text;
        StartCoroutine(LoadImage(imageName));
    }

    public void SnowProcess()
    {
        string imageName = text.text;
        StartCoroutine(LoadSnowImage(imageName));
    }

    #region Loading images and such

    IEnumerator LoadImage(string imageNameWithExtension)
    {
        string url = PlayerPrefs.GetString("Custom Path") + "/Custom Stuff/Custom Menu Backgrounds/" + imageNameWithExtension;

        byte[] imgData;
        Texture2D tex = new Texture2D(2, 2);

        //Check if we should use UnityWebRequest or File.ReadAllBytes
        if (url.Contains("://") || url.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
        }
        else
        {
            imgData = File.ReadAllBytes(url);
        }
        Debug.Log(imgData.Length);

        //Load raw Data into Texture2D 
        tex.LoadImage(imgData);

        //Convert Texture2D to Sprite
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), pivot, 100.0f);

        //Apply Sprite to SpriteRenderer
        Image image = customBackgroundObject.GetComponent<Image>();
        image.sprite = sprite;

    }

    IEnumerator LoadSnowImage(string imageNameWithExtension)
    {
        string url = PlayerPrefs.GetString("Custom Path") + "/Custom Stuff/Custom Snow Particles/" + imageNameWithExtension;

        byte[] imgData;
        Texture2D tex = new Texture2D(2, 2);

        //Check if we should use UnityWebRequest or File.ReadAllBytes
        if (url.Contains("://") || url.Contains(":///"))
        {
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();
            imgData = www.downloadHandler.data;
        }
        else
        {
            imgData = File.ReadAllBytes(url);
        }
        Debug.Log(imgData.Length);

        //Load raw Data into Texture2D 
        tex.LoadImage(imgData);

        //Apply Sprite to SpriteRenderer
        snowMaterial.mainTexture = tex;
        snowMaterial.SetTexture("_EmissionMap", tex);

    }

    #endregion

}
