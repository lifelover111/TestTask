using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Projectile : MonoBehaviour
{
    PhotonView photonView;
    Rigidbody2D rb;

    int _damage;
    float _projectileSpeed;
    Vector2 _direction;
    
    public Color color;

    public int damage { get { return _damage; } set { _damage = value; } }
    public float projectileSpeed { get { return _projectileSpeed; } set { _projectileSpeed = value; } }
    public Vector2 direction { get { return _direction; } set { _direction = value; } }

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        transform.SetParent(GameManager.instance.PROJECTILE_ANCHOR);
    }

    void Start()
    {
        if (photonView.IsMine)
        {
            photonView.RPC("SyncDamage", RpcTarget.All, damage);
            photonView.RPC("SyncColor", RpcTarget.All, new Vector3(color.r, color.g, color.b));
        }
        gameObject.GetComponent<SpriteRenderer>().color = color;
        rb.velocity = direction * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!photonView.IsMine)
            return;
        PhotonView pv;
        if(collision.gameObject.TryGetComponent(out pv))
        {
            if (PhotonNetwork.LocalPlayer != pv.Controller)
            {
                PhotonNetwork.Destroy(photonView);
            }
            else
                return;
        }
        else
        {
            PhotonNetwork.Destroy(photonView);
        }
    }


    [PunRPC]
    public void SyncDamage(int damage)
    {
        this.damage = damage;
    }
    [PunRPC]
    public void SyncColor(Vector3 color)
    {
        this.color = new Color(color.x, color.y, color.z);
    }
}
