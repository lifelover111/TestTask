using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] GameObject moveJoystickPrefab;
    [SerializeField] GameObject aimJoystickPrefab;
    [SerializeField] GameObject coinCounterPrefab;

    Player player;

    PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        if(photonView.IsMine)
        {
            CreateController();
            CreateUI();
        }
    }



    void CreateUI()
    {
        GameObject counter = Instantiate(coinCounterPrefab);
        counter.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
        counter.GetComponent<CoinCounter>().SetPlayer(player);
    }

    void CreateController()
    {
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).GetComponent<Player>();
        Canvas canvas = FindObjectOfType<Canvas>();
        GameObject joystick = Instantiate(moveJoystickPrefab);
        joystick.transform.SetParent(canvas.transform, false);
        player.SetMoveJoystick(joystick.GetComponent<Joystick>());
        joystick = Instantiate(aimJoystickPrefab);
        joystick.transform.SetParent(canvas.transform, false);
        joystick.transform.position = new Vector3(960, 0, 0);
        player.SetAimJoystick(joystick.GetComponent<Joystick>());
    }
}