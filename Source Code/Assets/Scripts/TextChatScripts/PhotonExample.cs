using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;

public class PhotonExample : MonoBehaviourPunCallbacks
{

    public Button submit;

    void Start()
    {
        submit.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined photon. and uh.. how much average ping photon?" + "lets see... about... " + PhotonNetwork.GetPing() + "... ping!");
        PhotonNetwork.NickName = "Player " + Random.Range(0, 9999).ToString("0000");
        PhotonNetwork.JoinRoom("Room", null);

        
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom("Room", null, null, null);
    }

    public override void OnJoinedRoom()
    {
        submit.interactable = true;
    }


}
