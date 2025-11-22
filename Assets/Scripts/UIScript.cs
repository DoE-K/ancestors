using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
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
}
