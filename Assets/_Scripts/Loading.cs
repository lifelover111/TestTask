using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    TMP_Text text;
    int points = 0;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        DontDestroyOnLoad(transform.parent.gameObject);
        text = GetComponent<TMP_Text>();
        InvokeRepeating("LoadingAnim", 0, 0.5f);
        SceneManager.LoadScene(1);
    }

    void LoadingAnim()
    {
        points++;
        points %= 4;
        string str = "";
        for(int i = 0; i < points; i++)
        {
            str += ".";
        }
        text.text = "Loading" + str;
    }
}
