using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreScript : MonoBehaviour
{
    public int score;

    public TMP_Text scoreTxt;

    void Start()
    {
        scoreTxt.text = "Score: " + PlayerPrefs.GetInt("Score");
    }

}

