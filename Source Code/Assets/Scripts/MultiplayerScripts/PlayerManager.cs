using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerManager : MonoBehaviour
{
    PhotonView PV;
    GameObject controller;
    PlayerController myPlayer;
    GameObject spectator;
    public GameObject spectatorPrefab;
    public List<string> _messages = new List<string>();
    private TMP_Text ChatContent;
    public int _maximumMessages = 8;
    private float buildDelay = 0.25f;
    float timer;
    private string killer;
    int deaths;
    [Tooltip("The code only supports up to 2 colors/materials. Adding more will result in the first 2 being used and the next not being used.")]
    public Material[] teams;
    

    private void Awake()
    {
        killer = "";
       PlayerPrefs.DeleteKey("Killer");
        deaths = 0;
        PV = GetComponent<PhotonView>();
        ChatContent = GameObject.FindGameObjectWithTag("ChatObject").GetComponent<TMP_Text>();
    }
    void Start()
    {
        if (PV.IsMine)
        {
            CreateController();
        }
    }

    // Update is called once per frame




    void CreateController()
    {
        //instantiate player ehehehehehehehehe

        GetPlayer();

    }

    public void Die()
    {
        StartCoroutine(DieCoroutine());
    }

    void GetPlayer()
    {
        Transform spawnpoint = SpawnManager.Instance.GetSpawnpoint();



        //camera mode players. do skins through RPCs. NOT through if statements.
        if(PlayerPrefs.HasKey("Camera Mode"))
        {
            if (PlayerPrefs.GetInt("Camera Mode") == 0)
            {
                //third per
                controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AllPlayers", "PlayerTP"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
                myPlayer = controller.GetComponent<PlayerController>();
                return;
            }
            else if (PlayerPrefs.GetInt("Camera Mode") == 1)
            {
                //first per
                controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AllPlayers", "PlayerFP"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
                myPlayer = controller.GetComponent<PlayerController>();
                return;
            }

        } else
        {
            controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "AllPlayers", "PlayerTP"), spawnpoint.position, spawnpoint.rotation, 0, new object[] { PV.ViewID });
            myPlayer = controller.GetComponent<PlayerController>();
            return;
        }

        //teams properties

        //myPlayer.TeamsCube.GetComponent<MeshRenderer>().material = teams[team];
    }



    public void MapFallingDeath()
    {
        killer = "World";
        PlayerPrefs.SetString("Killer", killer);
        PlayerPrefs.Save();
        StartCoroutine(FallingDieCoroutine());
    }



    IEnumerator DieCoroutine()
    {

        PhotonNetwork.Destroy(controller);
        CreateSpectator();
        yield return new WaitForSeconds(5.4f);
        Destroy(spectator);
        CreateController();
       PlayerPrefs.DeleteKey("Killer");

    }



    IEnumerator FallingDieCoroutine()
    {

        PhotonNetwork.Destroy(controller);
        CreateSpectator();
        yield return new WaitForSeconds(5.4f);
        Destroy(spectator);
        CreateController();
       PlayerPrefs.DeleteKey("Killer");

    }

    void CreateSpectator()
    {
        spectator = Instantiate(spectatorPrefab, transform.position, Quaternion.identity);
    }

    [PunRPC]
    void RPC_AddNewMessage(string msg)
    {
        _messages.Add(msg);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (PhotonNetwork.InRoom)
        {
            ChatContent.maxVisibleLines = _maximumMessages;

            if (_messages.Count > _maximumMessages)
            {
                _messages.RemoveAt(0);
            }

            if (buildDelay < Time.time)
            {
                BuildChatContents();
                buildDelay = Time.time + 0.25f;
            }
        }
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

}
