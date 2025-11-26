using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ScoreManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI highscoreText;

    [System.Serializable]
    public class HighscoreEntry
    {
        public string playerName;
        public int score;
    }

    [System.Serializable]
    public class HighscoreList
    {
        public List<HighscoreEntry> list;
    }

    public TMP_InputField nameInput;
    public string playerName = "XXX";

    void Start(){

        if (PlayerPrefs.HasKey("playerName"))
        {
            Debug.Log("Lade Name vro");
            playerName = PlayerPrefs.GetString("playerName");
        }
        
        if(GlobalScore.score > 0)
        {
            SaveFinalScore(playerName);
        }

        ShowHighscores();

    }

    public HighscoreList LoadHighscores()
    {
        string json = PlayerPrefs.GetString("highscores", "");

        if (string.IsNullOrEmpty(json))
        {
            return new HighscoreList { list = new List<HighscoreEntry>() };
        }

        return JsonUtility.FromJson<HighscoreList>(json);
    }

    public void AddHighscore(string name, int score)
    {
        HighscoreList highscores = LoadHighscores();

        highscores.list.Add(new HighscoreEntry
        {
            playerName = name,
            score = score
        });

        highscores.list = highscores.list
            .OrderByDescending(e => e.score)
            .ToList();

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscores", json);
        PlayerPrefs.Save();
    }

    public void ShowHighscores()
    {
        HighscoreList highscores = LoadHighscores();

        string lastName = PlayerPrefs.GetString("lastPlayerName", "");
        int lastScore = PlayerPrefs.GetInt("lastPlayerScore", -1);

        highscoreText.text = "";

        for (int i = 0; i < 5; i++)
        {
            if (i < highscores.list.Count)
            {
                var entry = highscores.list[i];
                highscoreText.text += $"{i + 1}. {entry.playerName} - {entry.score}\n";
            }
            else
            {
                // Platzhalter
                highscoreText.text += $"{i + 1}.\n";
            }
        }

        int placement = highscores.list.FindIndex(e =>
            e.playerName == lastName && e.score == lastScore
        );

        if (placement >= 5 && placement != -1)
        {
            highscoreText.text += "\n";
            highscoreText.text += $"{placement + 1}. {lastName} - {lastScore}";
        }
    }




    public bool IsNameValid()
    {
        string name = nameInput.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("Name darf nicht leer sein!");
            return false;
        }

        if (name.Length != 3)
        {
            Debug.Log("Name muss genau 3 Buchstaben haben!");
            return false;
        }

        foreach (char c in name)
        {
            if (!char.IsLetter(c))
            {
                Debug.Log("Name darf nur Buchstaben enthalten!");
                return false;
            }
        }

        return true;
    }

    public void OnSubmitName()
    {
        if (!IsNameValid())
            return;

        playerName = nameInput.text.ToUpper();

        PlayerPrefs.SetString("playerName", playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("map");
    }



    public void SaveFinalScore(string playerName)
    {
        int finalScore = GlobalScore.score;

        PlayerPrefs.SetString("lastPlayerName", playerName);
        PlayerPrefs.SetInt("lastPlayerScore", finalScore);

        AddHighscore(playerName, finalScore);

        GlobalScore.score = 0;
    }


    public void ResetHighscores()
    {
        PlayerPrefs.DeleteKey("highscores");
        PlayerPrefs.Save();
        highscoreText.text = 
        "1.\n" +
        "2.\n" +
        "3.\n" +
        "4.\n" +
        "5.";
    }

}

