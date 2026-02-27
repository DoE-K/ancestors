using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighscoreScript : MonoBehaviour
{
    //public int score;

    public TMP_Text scoreTxt;

    void Update()
    {
        scoreTxt.text = "Score: " + GlobalScore.score;
    }

}

