using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    PhotonView photonView;

    public Transform PROJECTILE_ANCHOR;
    public Transform COIN_ANCHOR;

    public List<Player> playerList;
    public List<PlayerData> playersData;

    Vector2[] playerStartPositions = new Vector2[] { new Vector2(-6, -3), new Vector2(6, 3), new Vector2(6, -3), new Vector2(-6, 3) };

    public int startCoinsNum = 10;

    float sizeX = 16;
    float sizeY = 9;


    public Color[] allAvailableColors = new Color[] { Color.red, Color.blue, Color.green, Color.yellow };
    public int[] appointedColors = new int[] { 0, 1, 2, 3 };

    bool isGameEnded = false;

    private void Awake()
    {
        instance = this;
        photonView = GetComponent<PhotonView>();

        PROJECTILE_ANCHOR = new GameObject("PROJECTILE_ANCHOR").transform;
        COIN_ANCHOR = new GameObject("COIN_ANCHOR").transform;

        playerList = new List<Player>();
        playersData = new List<PlayerData>();

        if(PhotonNetwork.IsMasterClient)
        {
            for(int i = 0; i < startCoinsNum; i++)
            {
                SpawnCoin();
            }

            InvokeRepeating("SpawnCoin", 6, 3);
        }
    }

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
            ShuffleColors();
    }

    public Vector2 GetStartPosition()
    {
        return playerStartPositions[PhotonNetwork.LocalPlayer.ActorNumber - 1];
    }

    public void RemovePlayer(Player player)
    {
        if (!isGameEnded)
        {
            playerList.Remove(player);
            if (playerList.Count < 2)
            {
                isGameEnded = true;
                foreach (Player p in playerList)
                {
                    p.OffJoysticks();
                    playersData.Add(p.data);
                }
                Invoke("EndGame", 1f);
            }
        }
    }


    void EndGame()
    {
        Leaderboard.instance.gameObject.SetActive(true);
        Leaderboard.instance.FillTable(playersData);
    }

    void SpawnCoin()
    {
        PhotonNetwork.Instantiate("Coin", new Vector2(Random.Range(-sizeX / 2, sizeX / 2), Random.Range(-sizeY / 2, sizeY / 2)), Quaternion.identity);
    }

    void ShuffleColors()
    {
        System.Random random = new System.Random();
        for (int i = appointedColors.Length - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);
            var temp = appointedColors[j];
            appointedColors[j] = appointedColors[i];
            appointedColors[i] = temp;
        }
        photonView.RPC("SyncColorIndexes", RpcTarget.All, appointedColors);
    }

    [PunRPC]
    public void SyncColorIndexes(int[] indexes)
    {
        appointedColors = indexes;
    }
}

