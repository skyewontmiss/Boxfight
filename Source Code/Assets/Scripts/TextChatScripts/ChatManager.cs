using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Realtime;

public class ChatManager : MonoBehaviour
{

    public TMP_InputField ChatInput;
    public TextMeshProUGUI ChatContent;
    public PhotonView PV;
    List<string> _messages = new List<string>();
    private int _maximumMessages = 8;
    private float buildDelay = 0f;

    void Start()
    {

    }


    public void SendChat(string msg)
    {
        string newMessage = PhotonNetwork.NickName + ":" + msg;
        PV.RPC("RPC_AddNewMessage", RpcTarget.All, newMessage);
    }

    [PunRPC]
    void RPC_AddNewMessage(string msg)
    {
        _messages.Add(msg);
    }

    public void SubmitChat()
    {
        SendChat(ChatInput.text);
        ChatInput.ActivateInputField();
        ChatInput.text = "";
    }

    void BuildChatContents()
    {
        string newContents = "";
        foreach (string msgs in _messages)
        {
            newContents += msgs + "\n";
        }

        ChatContent.text = newContents;
    }

    // Update is called once per frame
    void Update()
    {

       // uncomment the following if statement when integrated into game
        if (PhotonNetwork.InRoom)
        {
            ChatContent.maxVisibleLines = _maximumMessages;

            if (_messages.Count > _maximumMessages)
            {
                _messages.RemoveAt(0);
            }

            if(buildDelay < Time.time)
            {
                BuildChatContents();
                buildDelay = Time.time + 0.25f;
            }


       }
       else if (_messages.Count > 0)
       {
            _messages.Clear();
            ChatContent.text = "";
        }
    }
}
