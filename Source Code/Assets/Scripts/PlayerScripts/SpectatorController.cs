using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpectatorController : MonoBehaviour
{
    public TMP_Text respawnText, killedByText;
    

    private void Start()
    {
        if (PlayerPrefs.GetString("Killer") == "World")
        {
            killedByText.text = "You fell outta the map. Don't do that again!";

        }
        else if (string.IsNullOrEmpty(PlayerPrefs.GetString("Killer")))
        {
            killedByText.text = "Error BX06: Failed to fetch killer data.";
        }
        else

        {
            killedByText.text = "Killed by " + PlayerPrefs.GetString("Killer") + ". Don't die again!";

        }

        StartCoroutine(RespawnCoroutine());
    }

    IEnumerator RespawnCoroutine()
    {
        respawnText.text = "5";
        yield return new WaitForSeconds(1f);
        respawnText.text = "4";
        yield return new WaitForSeconds(1f);
        respawnText.text = "3";
        yield return new WaitForSeconds(1f);
        respawnText.text = "2";
        yield return new WaitForSeconds(1f);
        respawnText.text = "1";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.9";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.8";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.7";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.6";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.5";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.4";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.3";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.2";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.1";
        yield return new WaitForSeconds(0.1f);
        respawnText.text = "0.0";
    }
}
