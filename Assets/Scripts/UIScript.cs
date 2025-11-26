using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public GameObject tutorialtxt;
    private bool tutactive;

    public void goHighscore()
    {
        SceneManager.LoadScene("highscore");
    }

    public void goMap()
    {
        SceneManager.LoadScene("map");   
    }

    public void goMenu()
    {
        SceneManager.LoadScene("menu");
    }

    public void goLoserMenu()
    {
        GlobalScore.score = 0;
        SceneManager.LoadScene("menu");
    }

    public void SetTutActive()
    {
        if(tutactive == false){
            tutorialtxt.SetActive(true);
            tutactive = true;
        }
        else 
        {
            tutorialtxt.SetActive(false);
            tutactive = false;
        }
    }
}
