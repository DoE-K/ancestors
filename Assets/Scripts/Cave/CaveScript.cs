using UnityEngine;
using UnityEngine.SceneManagement;

public class CaveScript : MonoBehaviour
{
    public string caveName;
    public bool isExit; 

    //public string returnSpawnName; 

    /*private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("CaveName", caveName);

            SceneManager.LoadScene("cave");
        }

        if (other.CompareTag("Player") && isExit == true)
        {
            PlayerPrefs.SetString("MapSpawnName", returnSpawnName);
            SceneManager.LoadScene("MapScene");
        }
    }*/
}
