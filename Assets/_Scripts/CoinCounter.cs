using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    TMP_Text counter;
    Player player;
    private void Awake()
    {
        counter = GetComponentInChildren<TMP_Text>();
    }
    private void FixedUpdate()
    {
        counter.text = player.GetCoinsNumber().ToString();
    }

    public void SetPlayer(Player player)
    {
        this.player = player;
    }
}
