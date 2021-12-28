using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Photon.Realtime;
using Photon.Pun;

public class FailsafeManager : MonoBehaviour
{

    private string url = "https://raw.githubusercontent.com/Kaylerr/Boxfight-Update-Data/main/update%20data/version";
    private string updateLink = "https://kaylerr.itch.io/shoot/purchase";

    private float progress;

    private string latestVersion;
    public string currentVersion;

    public GameObject blockObject;

    private void Start()
    {
        StartCoroutine(GetText());
        currentVersion = currentVersion + "\n";
    }

    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            latestVersion = www.downloadHandler.text;
            GameVersionCheck();
        }
    }

    public void GameVersionCheck()
    {
        Debug.Log(currentVersion);
        Debug.Log(latestVersion);
        if (latestVersion == currentVersion)
        {
            blockObject.SetActive(false);

            Debug.Log("This version " + "(" + currentVersion + ") is fine.");
        } else
        {
            blockObject.SetActive(true);
            Debug.LogError("AN ERROR OCCURRED: " + currentVersion + " is NOT the latest version. The latest version is: " + latestVersion + ". Update to get new features or quit the game!");
            // if they are not on the latest version, we disconnect them from Photon, thus not allowing them to do anything.
            PhotonNetwork.Disconnect();
        }
    }

    public void UpdateVersion()
    {
        GetUpdate();
    }

    void GetUpdate()
    {
        Application.OpenURL(updateLink);
        Application.Quit();
    }
}