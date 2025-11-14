using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    private GameDataManager gameDataManager;

    //public ScoreManager 

    void Start()
    {
        
        gameDataManager = FindObjectOfType<GameDataManager>();
    }

    public void reset()
    {
        SaveSystem.ResetSave();
    }

    public void goHighscore()
    {
        SceneManager.LoadScene("highscore");
    }

    public void goMap()
    {
        /*if (gameDataManager != null)
        {
            gameDataManager.SaveGame();
        }*/

        SceneManager.LoadScene("map");
        
    }

    public void goMenu()
    {
        SceneManager.LoadScene("menu");
    }
}
