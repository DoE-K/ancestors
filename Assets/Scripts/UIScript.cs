using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    private GameDataManager gameDataManager;

    void Start()
    {
        
        gameDataManager = FindObjectOfType<GameDataManager>();
    }

    public void reset()
    {
        SaveSystem.ResetSave();
    }

    public void goSettings()
    {
        if (gameDataManager != null)
        {
            gameDataManager.SaveGame();
        }
        SceneManager.LoadScene("settings");
    }

    public void goMap()
    {
        if (gameDataManager != null)
        {
            gameDataManager.SaveGame();
        }
        SceneManager.LoadScene("map");
    }
}
