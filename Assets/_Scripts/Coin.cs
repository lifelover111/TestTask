using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coin : MonoBehaviour
{
    PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        transform.SetParent(GameManager.instance.COIN_ANCHOR);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Projectile")
            return;
        if(photonView.IsMine)
            PhotonNetwork.Destroy(photonView);
    }
}
