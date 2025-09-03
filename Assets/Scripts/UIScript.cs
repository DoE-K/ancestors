using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reset()
    {
        SaveSystem.ResetSave();
    }

    public void goSettings()
    {
        player.SaveGame();
        SceneManager.LoadScene("settings");
    }

    public void goMap()
    {
        player.SaveGame();
        SceneManager.LoadScene("map");
    }
}
