using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public Button MultiplayerButton, errorReturnButton;
    public GameObject ConnectionObject;
    [SerializeField] TMP_InputField roomNameInputField;
    public TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform playerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    public GameObject FindRoomMenu;
    public GameObject CreateRoomMenu;
    [SerializeField] GameObject[] hostControls;
    [SerializeField] Animator animatedPanelSceneSwitcher;
    [SerializeField] TMP_Text playerCount;
    [SerializeField] TMP_Text playerVersionWelcome;

    //for festivity
    [SerializeField] public Image[] ChristmasTrees;



    public static Launcher instance;
    int Map;
    string gameVersion;


    private void Awake()
    {
        instance = this;
        Map = 3;
        gameVersion = playerVersionWelcome.text;
    }

     void Start()
     {
           ConnectToPhoton();
     }

    // Start is called before the first frame update
    void ConnectToPhoton()
    {
        ConnectingToPhoton();
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
        playerCount.text = "Players online: " + "\n" + PhotonNetwork.CountOfPlayers;
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("joined photon. and uh.. how much average ping photon?" + "lets see... about... " + PhotonNetwork.GetPing() + "... ping!");
        ConnectedToPhoton();
        if(PlayerPrefs.HasKey("Username"))
        {
            RefreshMenuUsername();

        } else
        {
            CreateMenuUsername();
        }

        PhotonNetwork.AutomaticallySyncScene = true;

    }

    public void RefreshMenuUsername()
    {
        PhotonNetwork.NickName = PlayerPrefs.GetString("Username");

        playerVersionWelcome.text = gameVersion + " - Welcome, " + PhotonNetwork.NickName + ".";
    }

    public void CreateMenuUsername()
    {
        PhotonNetwork.NickName = "Player " + Random.Range(0, 9999).ToString("0000");
        PlayerPrefs.SetString("Username", PhotonNetwork.NickName);
        playerVersionWelcome.text = gameVersion + " - Welcome, " + PhotonNetwork.NickName + ".";
        PlayerPrefs.Save();
    }

    public void SinglePlayerCampaign()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();

        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(2);
        RoomManager.instance.OnDestroy();
    }

    public void LocalScene(string sceneToSwitchTo)
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            PhotonNetwork.LeaveRoom();

        }

        PhotonNetwork.Disconnect();
        SceneManager.LoadScene(sceneToSwitchTo);
        RoomManager.instance.OnDestroy();
    }



    public void SelectMap(int mapSceneToSelect)
    {
        Map = mapSceneToSelect;
    }
    

    public void ConnectingToPhoton()
    {
            MultiplayerButton.interactable = false;
            ConnectionObject.SetActive(true);
    }

    public void ConnectedToPhoton()
    {
            MultiplayerButton.interactable = true;
            ConnectionObject.SetActive(false);
    }


    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text))
        {
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room");
            MenuManager.Instance.OpenMenu("Loading");
        }

        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnJoinedRoom()
    {
        MenuManager.Instance.OpenMenu("Room Menu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Count(); i++)
        {
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(players[i]);
        }

        foreach (GameObject hostVisuals in hostControls)
        {
            hostVisuals.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        foreach (GameObject hostVisuals in hostControls)
        {
            hostVisuals.SetActive(PhotonNetwork.IsMasterClient);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorReturnButton.enabled = true;
        errorText.text = "error: " + message + ". Error Code: " + returnCode;
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("Loading");
    }

    public override void OnLeftRoom()
    {
        MenuManager.Instance.OpenMenu("Find Room");
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManager.Instance.OpenMenu("Loading");
        FindRoomMenu.SetActive(false);
        CreateRoomMenu.SetActive(false);

    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(Map);
    }

    public void AppQuit()
    {
        Application.Quit();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        {
            foreach (Transform transform in roomListContent)
            {
                Destroy(transform.gameObject);
            }

            for (int i = 0; i < roomList.Count; i++)
            {
                if (roomList[i].RemovedFromList)
                    continue;
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
            }
        }
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().Setup(newPlayer);
    }

    //a type of menu room kick system. I think.



    















    //section for menu manager
}
