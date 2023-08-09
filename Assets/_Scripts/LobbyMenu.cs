using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class LobbyMenu : MonoBehaviour
{
    public static LobbyMenu instance;


    public string roomName;
    TMP_Text roomNameText;

    [SerializeField] Transform[] playersPos;

    public Transform createMenu;
    public Transform joinMenu;
    Transform roomMenu;
    public Transform startGameButton;

    TMP_InputField _createInputField;
    TMP_InputField _joinInputField;

    public TMP_InputField createInputField { get { return _createInputField; } private set { _createInputField = value; } }
    public TMP_InputField joinInputField { get { return _joinInputField; } private set { _joinInputField = value; } }

    public Transform loading;
    public Transform canvas;

    private void Awake()
    {
        instance = this;
        canvas = transform.parent;
        createMenu = transform.Find("CreateMenu");
        joinMenu = transform.Find("JoinMenu");
        roomMenu = transform.Find("RoomMenu");
        roomNameText = roomMenu.Find("RoomName").gameObject.GetComponent<TMP_Text>();
        startGameButton = roomMenu.Find("StartGame");
        createInputField = createMenu.Find("CreateInputField").gameObject.GetComponent<TMP_InputField>();
        joinInputField = joinMenu.Find("JoinInputField").gameObject.GetComponent<TMP_InputField>();

        loading = GameObject.Find("LoadingCanvas")?.transform;

        createMenu.gameObject.SetActive(false);
        joinMenu.gameObject.SetActive(false);

        canvas.gameObject.SetActive(false);
    }


    public void AddPlayerInCurrentRoom()
    {
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            playersPos[i].gameObject.SetActive(true);
            TMP_Text tmpText = playersPos[i].gameObject.GetComponent<TMP_Text>();
            tmpText.text = PhotonNetwork.CurrentRoom.Players[i + 1].NickName;
            if (PhotonNetwork.CurrentRoom.Players[i + 1] == PhotonNetwork.LocalPlayer)
                tmpText.fontStyle = FontStyles.Bold;
        }
    }

    public void ShowRoom()
    {
        roomName = PhotonNetwork.CurrentRoom.Name;
        createMenu.gameObject.SetActive(false);
        joinMenu.gameObject.SetActive(false);
        roomMenu.gameObject.SetActive(true);
        roomNameText.text = "Room: " + roomName;
    }


    public void StartGame()
    {
        PhotonNetwork.LoadLevel(2);
    }
}
