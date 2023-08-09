using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    GameObject player;
    TextMesh text;
    private void Awake()
    {
        text = GetComponent<TextMesh>();
    }

    private void Start()
    {
        player = transform.parent.gameObject;
    }
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, -player.transform.rotation.z);
    }

    public void SetValue(int health)
    {
        text.text = health.ToString();
    }
}
