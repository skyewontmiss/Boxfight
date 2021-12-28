using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviourPunCallbacks
{

    bool isShowing;
    [SerializeField] Transform container;
    [SerializeField] GameObject scoreboardItemPrefab, scoreboardItemHolder;

    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();

    private void Awake()
    {
        isShowing = false;
    }

    private void Start()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }


    void Update()
    {
        ShowHide();
    }

    void ShowHide()
    {
        if (Input.GetKey(KeyCode.Tab))
        {
            isShowing = true;

        } else
        {
            isShowing = false;
        }

        if (isShowing)
        {
            scoreboardItemHolder.SetActive(true);
        }
        else
        {
            scoreboardItemHolder.SetActive(false);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    void AddScoreboardItem(Player player)
    {
        ScoreboardItem item = Instantiate(scoreboardItemPrefab, container).GetComponent<ScoreboardItem>();
        item.Initialize(player);
        scoreboardItems[player] = item;
    }

    void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }


}
