using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player : MonoBehaviour
{
    [SerializeField] GameObject healthbarPrefab;

    Healthbar healthbar;

    Joystick moveJoystick;
    Joystick aimJoystick;

    Weapon weapon;
    Rigidbody2D rb;
    public PhotonView photonView;

    public Color color;
    SpriteRenderer[] sRends;

    int health = 10;
    int coins = 0;
    float speed = 2f;
    float shootDelay = 1f;
    float timeShotDone;
    float timeDamageTaken;

    public PlayerData data;

    bool isGameStarted = false;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody2D>();
        weapon = transform.GetChild(0).gameObject.GetComponent<Weapon>();
        transform.position = GameManager.instance.GetStartPosition();
        GameManager.instance.playerList.Add(this);
        CreateHealthbar();
        sRends = gameObject.GetComponentsInChildren<SpriteRenderer>();
    }
    
    private void Start()
    {
        Invoke("AppointColors", 0.3f);
        Invoke("StartAfterSync", 0.6f);
    }

    void AppointColors()
    {
        if (photonView.IsMine)
        {
            color = GameManager.instance.allAvailableColors[GameManager.instance.appointedColors[PhotonNetwork.LocalPlayer.ActorNumber - 1]];
            photonView.RPC("SyncColor", RpcTarget.All, new Vector3(color.r, color.g, color.b));
        }
    }

    void StartAfterSync()
    {
        isGameStarted = true;
        foreach (SpriteRenderer sr in sRends)
            sr.color = color;
        GetComponentInChildren<Weapon>().RefreshColor();
        data = new PlayerData();
        data.name = photonView.Controller.NickName;
        data.color = color;
        data.coins = coins;
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;
        rb.velocity = speed * moveJoystick.Direction;

        Vector2 viewDirection = aimJoystick.Direction;
        if (viewDirection.magnitude != 0)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, viewDirection);
            if (viewDirection.magnitude >= 1 && (Time.time - timeShotDone >= shootDelay))
            { 
                weapon.Shoot(viewDirection);
                timeShotDone = Time.time;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile")
            return;
        if (!photonView.IsMine)
            return;
        if (Time.time - timeDamageTaken < 0.2f)
            return;
        timeDamageTaken = Time.time;
        PhotonView pv = collision.gameObject.GetComponent<PhotonView>();
        if (photonView.Controller != pv.Controller)
        {
            TakeDamage(collision.gameObject.GetComponent<Projectile>().damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isGameStarted)
            return;
        if (!photonView.IsMine)
            return;
        coins++;
        photonView.RPC("SyncData", RpcTarget.All, coins);
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        photonView.RPC("SyncHealth", RpcTarget.All, health);
        healthbar.SetValue(health);
        if (health <= 0)
            Die();
    }

    [PunRPC]
    public void SyncHealth(int health)
    {
        this.health = health;
        healthbar.SetValue(health);
    }
    [PunRPC]
    public void SyncColor(Vector3 color)
    {
        this.color = new Color(color.x, color.y, color.z);
    }
    [PunRPC]
    public void SyncData(int coins)
    {
        data.coins = coins;
    }

    void Die()
    {
        OffJoysticks();
        PhotonNetwork.Destroy(photonView);
    }

    private void OnDestroy()
    {
        GameManager.instance?.RemovePlayer(this);
        GameManager.instance?.playersData.Add(data);
    }

    void CreateHealthbar()
    {
        GameObject healthbar = Instantiate(healthbarPrefab);
        healthbar.transform.SetParent(transform);
        healthbar.transform.localPosition = Vector3.zero;
        this.healthbar = healthbar.GetComponent<Healthbar>();
    }

    public int GetCoinsNumber()
    {
        return coins;
    }
    public int GetHealth()
    {
        return health;
    }

    public void OffJoysticks()
    {
        moveJoystick?.gameObject.SetActive(false);
        aimJoystick?.gameObject.SetActive(false);
    }

    public void SetMoveJoystick(Joystick joystick)
    {
        moveJoystick = joystick;
    }

    public void SetAimJoystick(Joystick joystick)
    {
        aimJoystick = joystick;
    }
}

public class PlayerData
{
    public string name;
    public Color color;
    public int coins;
}