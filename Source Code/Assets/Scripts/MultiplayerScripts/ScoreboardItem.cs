using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText, killsText, deathText;

    public void Initialize(Player player)
    {
        usernameText.text = player.NickName;
    }


}
