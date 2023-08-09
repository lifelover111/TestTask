using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class Leaderboard : MonoBehaviour
{
    public static Leaderboard instance;
    
    [SerializeField] GameObject rowPrefab;

    bool leaderboardIsExist = false;

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }

    public void FillTable(List<PlayerData> data)
    {
        if (leaderboardIsExist)
            return;

        data.OrderByDescending(a => a.coins);
        for (int i = 0; i < data.Count; i++)
        {
            GameObject go = Instantiate(rowPrefab);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = new Vector3(0, 200 - i * 100, 0);
            TMP_Text rank = go.transform.GetChild(0).GetComponent<TMP_Text>();
            rank.text = (i + 1) + ".";
            TMP_Text player = go.transform.GetChild(1).GetComponent<TMP_Text>();
            player.text = data[i].name;
            player.faceColor = data[i].color;
            TMP_Text score = go.transform.GetChild(2).GetComponent<TMP_Text>();
            score.text = data[i].coins.ToString();
            score.fontStyle = FontStyles.Bold;
        }
        leaderboardIsExist = true;
    }

    public void BackToLobbyMenu()
    {
        bool masterClientInRoom = false;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
            if (PhotonNetwork.PlayerList[i].IsMasterClient)
                masterClientInRoom = true;
        if (!PhotonNetwork.IsMasterClient && masterClientInRoom)
            return;
        /// ¬ случае, когда игрок-хост становитс€ победителем, если другой игрок выйдет раньше,
        /// чем хост, у него по какой-то причине начинает загружатьс€ сцена игры, а не лобби,
        /// из-за чего возникает р€д ошибок, поэтому выше € отключил возможность нажати€ кнопки выхода
        /// до того, как хост выйдет из комнаты

        
        PhotonNetwork.LeaveRoom();
        Destroy(RoomManager.instance.gameObject);
        PhotonNetwork.AutomaticallySyncScene = false;
        
        PhotonNetwork.LoadLevel(1);
    }
}
