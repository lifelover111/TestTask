using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        Destroy(LobbyMenu.instance.loading?.gameObject);
        LobbyMenu.instance.canvas.gameObject.SetActive(true);
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.NickName = "Player" + Random.Range(0, 999);
        LobbyMenu.instance.createMenu.gameObject.SetActive(true);
        LobbyMenu.instance.joinMenu.gameObject.SetActive(true);
    }

    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(LobbyMenu.instance.createInputField.text))
        {
            return;
        }
        Photon.Realtime.RoomOptions options = new Photon.Realtime.RoomOptions();
        options.MaxPlayers = 4;
        PhotonNetwork.CreateRoom(LobbyMenu.instance.createInputField.text, options);
        LobbyMenu.instance.createMenu.gameObject.SetActive(false);
        LobbyMenu.instance.joinMenu.gameObject.SetActive(false);
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(LobbyMenu.instance.joinInputField.text);
        LobbyMenu.instance.createMenu.gameObject.SetActive(false);
        LobbyMenu.instance.joinMenu.gameObject.SetActive(false);
    }

    public override void OnJoinedRoom()
    {
        LobbyMenu.instance.ShowRoom();
        LobbyMenu.instance.AddPlayerInCurrentRoom();
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log(message);
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        LobbyMenu.instance.AddPlayerInCurrentRoom();
        if(PhotonNetwork.CurrentRoom.PlayerCount > 1 && PhotonNetwork.IsMasterClient)
            LobbyMenu.instance.startGameButton.gameObject.SetActive(true);
    }
}
