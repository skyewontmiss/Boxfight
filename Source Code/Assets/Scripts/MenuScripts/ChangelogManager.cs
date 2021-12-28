using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class ChangelogManager : MonoBehaviour
{


    private string url;

    private float progress;

    string changelogTitle, changelogContent;

    public TMP_Text changelogContentText, changelogTitleText;


    private void Start()
    {

    }

    IEnumerator GetChangelogFromGithub()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
            changelogTitleText.text = "Error BX_404: error getting changelog title.";
            changelogContentText.text = "Error BX_405: getting changelog contents.";
        }
        else
        {
            changelogContent = www.downloadHandler.text;
            ChangelogUpdate();
        }
    }

    public void ChangelogUpdate()
    {
        changelogTitleText.text = changelogTitle;
        changelogContentText.text = changelogContent;
    }
    public void ChangelogContent(string urlInput)
    {
        url = urlInput;
        StartCoroutine(GetChangelogFromGithub());
    }

    public void ChangelogTitle(string ChangelogTitle)
    {
        changelogTitle = ChangelogTitle;
    }

}