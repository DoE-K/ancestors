using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private const string HighscoresKey   = "highscores";
    private const string PlayerNameKey   = "playerName";
    private const string LastNameKey     = "lastPlayerName";
    private const string LastScoreKey    = "lastPlayerScore";
    private const string DefaultName     = "XXX";
    private const int    MaxNameLength   = 3;
    private const int    TopEntriesToShow = 5;

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI _highscoreText;
    [SerializeField] private TMP_InputField  _nameInput;

    private string _playerName = DefaultName;

    // ── Unity lifecycle ──────────────────────────────────────────────────────

    private void Start()
    {
        _playerName = PlayerPrefs.GetString(PlayerNameKey, DefaultName);

        if (GlobalScore.Score > 0)
            SaveFinalScore(_playerName);

        ShowHighscores();
    }

    // ── Public UI callbacks (assign in Inspector) ────────────────────────────

    public void OnSubmitName()
    {
        if (!IsNameValid()) return;

        _playerName = _nameInput.text.Trim().ToUpper();
        PlayerPrefs.SetString(PlayerNameKey, _playerName);
        PlayerPrefs.Save();

        SceneManager.LoadScene("map");
    }

    public void ResetHighscores()
    {
        PlayerPrefs.DeleteKey(HighscoresKey);
        PlayerPrefs.Save();
        DisplayEmptyLeaderboard();
    }

    // ── Highscore persistence ────────────────────────────────────────────────

    public void SaveFinalScore(string name)
    {
        int finalScore = GlobalScore.Score;

        PlayerPrefs.SetString(LastNameKey, name);
        PlayerPrefs.SetInt(LastScoreKey, finalScore);
        PlayerPrefs.Save();

        AddHighscore(name, finalScore);

        GlobalScore.Reset();
    }

    public void AddHighscore(string name, int score)
    {
        var highscores = LoadHighscores();

        highscores.list.Add(new HighscoreEntry { playerName = name, score = score });
        highscores.list = highscores.list
            .OrderByDescending(e => e.score)
            .ToList();

        PlayerPrefs.SetString(HighscoresKey, JsonUtility.ToJson(highscores));
        PlayerPrefs.Save();
    }

    public HighscoreList LoadHighscores()
    {
        string json = PlayerPrefs.GetString(HighscoresKey, "");

        return string.IsNullOrEmpty(json)
            ? new HighscoreList { list = new List<HighscoreEntry>() }
            : JsonUtility.FromJson<HighscoreList>(json);
    }

    // ── Display ──────────────────────────────────────────────────────────────

    public void ShowHighscores()
    {
        var highscores = LoadHighscores();
        string lastName  = PlayerPrefs.GetString(LastNameKey, "");
        int    lastScore = PlayerPrefs.GetInt(LastScoreKey, -1);

        var sb = new System.Text.StringBuilder();

        for (int i = 0; i < TopEntriesToShow; i++)
        {
            if (i < highscores.list.Count)
            {
                var e = highscores.list[i];
                sb.AppendLine($"{i + 1}. {e.playerName} - {e.score}");
            }
            else
            {
                sb.AppendLine($"{i + 1}.");
            }
        }

        // Show the player's own entry if it falls outside the top 5
        int placement = highscores.list.FindIndex(
            e => e.playerName == lastName && e.score == lastScore
        );

        if (placement >= TopEntriesToShow && placement != -1)
        {
            sb.AppendLine();
            sb.Append($"{placement + 1}. {lastName} - {lastScore}");
        }

        _highscoreText.text = sb.ToString();
    }

    // ── Name validation ──────────────────────────────────────────────────────

    public bool IsNameValid()
    {
        string name = _nameInput.text.Trim();

        if (string.IsNullOrEmpty(name))
        {
            Debug.Log("[ScoreManager] Name darf nicht leer sein.");
            return false;
        }

        if (name.Length != MaxNameLength)
        {
            Debug.Log($"[ScoreManager] Name muss genau {MaxNameLength} Buchstaben haben.");
            return false;
        }

        if (!name.All(char.IsLetter))
        {
            Debug.Log("[ScoreManager] Name darf nur Buchstaben enthalten.");
            return false;
        }

        return true;
    }

    // ── Nested data classes ──────────────────────────────────────────────────

    [System.Serializable]
    public class HighscoreEntry
    {
        public string playerName;
        public int    score;
    }

    [System.Serializable]
    public class HighscoreList
    {
        public List<HighscoreEntry> list;
    }

    // ── Private helpers ──────────────────────────────────────────────────────

    private void DisplayEmptyLeaderboard()
    {
        var sb = new System.Text.StringBuilder();
        for (int i = 1; i <= TopEntriesToShow; i++)
            sb.AppendLine($"{i}.");
        _highscoreText.text = sb.ToString();
    }
}