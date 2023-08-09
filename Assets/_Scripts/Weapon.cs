using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
    int damage = 1;
    float projectileSpeed = 10f;
    Color color;

    private void Start()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void RefreshColor()
    {
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    public void Shoot(Vector2 direction)
    {
        GameObject go = PhotonNetwork.Instantiate("Projectile", transform.position, Quaternion.identity);
        Projectile projectile = go.GetComponent<Projectile>();
        projectile.damage = damage;
        projectile.projectileSpeed = projectileSpeed;
        projectile.direction = direction.normalized;
        projectile.color = color;
    }
}
