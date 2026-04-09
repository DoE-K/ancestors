using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Game Over / Highscore screen. Arcade style.
///
/// Flow:
///   1. If score > 0 → show name input panel
///   2. Player enters 3-letter name, confirms
///   3. Score saved to PlayerPrefs
///   4. Highscore list shown with player's entry highlighted
///   5. Play Again / Quit buttons
///
/// Scene setup:
///   - NameInputPanel  (active by default)
///   - HighscorePanel  (inactive by default)
/// </summary>
public class GameOverUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject _nameInputPanel;
    [SerializeField] private GameObject _highscorePanel;

    [Header("Name Input")]
    [SerializeField] private TMP_InputField _nameInput;
    [SerializeField] private TMP_Text       _scorePreviewText;
    [SerializeField] private TMP_Text       _nameErrorText;
    [SerializeField] private Button         _confirmButton;

    [Header("Highscore List")]
    [SerializeField] private TMP_Text _highscoreText;
    [SerializeField] private TMP_Text _yourScoreText;

    [Header("Buttons")]
    [SerializeField] private Button _playAgainButton;
    [SerializeField] private Button _quitButton;

    [Header("Scene Names")]
    [SerializeField] private string _gameSceneName = "map";

    private const string HighscoresKey  = "highscores";
    private const string PlayerNameKey  = "playerName";
    private const int    MaxNameLength  = 3;
    private const int    TopEntries     = 5;

    private int    _finalScore;
    private string _playerName = "AAA";

    // ── Unity lifecycle ───────────────────────────────────────────────────────

    private void Start()
    {
        _finalScore = GlobalScore.Score;

        // If no score (e.g. opened scene directly) skip name input
        if (_finalScore <= 0)
        {
            ShowHighscorePanel();
            return;
        }

        // Load previously saved name as default
        _playerName = PlayerPrefs.GetString(PlayerNameKey, "AAA");
        _nameInput.text = _playerName;
        _nameInput.characterLimit = MaxNameLength;

        _scorePreviewText.text = $"YOUR SCORE\n{_finalScore}";
        _nameErrorText.text    = "";

        _nameInputPanel.SetActive(true);
        _highscorePanel.SetActive(false);

        _confirmButton.onClick.AddListener(OnConfirmName);
        _playAgainButton.onClick.AddListener(OnPlayAgain);
        _quitButton.onClick.AddListener(OnQuit);

        // Auto-select the input field
        _nameInput.Select();
        _nameInput.ActivateInputField();
    }

    // ── Button callbacks ──────────────────────────────────────────────────────

    public void OnConfirmName()
    {
        string name = _nameInput.text.Trim().ToUpper();

        if (!IsNameValid(name))
            return;

        _playerName = name;
        PlayerPrefs.SetString(PlayerNameKey, _playerName);

        SaveScore(_playerName, _finalScore);
        GlobalScore.Reset();

        ShowHighscorePanel();
    }

    public void OnPlayAgain()
    {
        SceneManager.LoadScene(_gameSceneName);
    }

    public void OnQuit()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    // ── Private ───────────────────────────────────────────────────────────────

    private void ShowHighscorePanel()
    {
        _nameInputPanel.SetActive(false);
        _highscorePanel.SetActive(true);

        BuildHighscoreDisplay();
    }

    private void BuildHighscoreDisplay()
    {
        var list = LoadHighscores();
        var sb   = new System.Text.StringBuilder();

        // Header — arcade style all caps
        sb.AppendLine("--- HIGH SCORES ---\n");

        for (int i = 0; i < TopEntries; i++)
        {
            if (i < list.Count)
            {
                var e       = list[i];
                bool isYou  = e.playerName == _playerName && e.score == _finalScore && _finalScore > 0;
                string line = $"{i + 1}.  {e.playerName}  {e.score:D6}";

                // Mark the player's own entry
                sb.AppendLine(isYou ? $"> {line} <" : $"  {line}");
            }
            else
            {
                sb.AppendLine($"  {i + 1}.  ---  000000");
            }
        }

        _highscoreText.text = sb.ToString();

        // Show the player's own rank if outside top 5
        if (_finalScore > 0)
        {
            int rank = list.FindIndex(e => e.playerName == _playerName && e.score == _finalScore);
            if (rank >= TopEntries)
                _yourScoreText.text = $"YOUR RANK: {rank + 1}  {_playerName}  {_finalScore:D6}";
            else
                _yourScoreText.text = "";
        }
    }

    private bool IsNameValid(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            _nameErrorText.text = "NAME CANNOT BE EMPTY";
            return false;
        }

        if (name.Length != MaxNameLength)
        {
            _nameErrorText.text = $"NAME MUST BE {MaxNameLength} LETTERS";
            return false;
        }

        if (!name.All(char.IsLetter))
        {
            _nameErrorText.text = "LETTERS ONLY";
            return false;
        }

        _nameErrorText.text = "";
        return true;
    }

    // ── Persistence ───────────────────────────────────────────────────────────

    private void SaveScore(string name, int score)
    {
        var list = LoadHighscores();

        list.Add(new HighscoreEntry { playerName = name, score = score });
        list = list.OrderByDescending(e => e.score).ToList();

        var data = new HighscoreList { list = list };
        PlayerPrefs.SetString(HighscoresKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    private List<HighscoreEntry> LoadHighscores()
    {
        string json = PlayerPrefs.GetString(HighscoresKey, "");
        if (string.IsNullOrEmpty(json))
            return new List<HighscoreEntry>();

        return JsonUtility.FromJson<HighscoreList>(json)?.list
               ?? new List<HighscoreEntry>();
    }

    // ── Nested data classes ───────────────────────────────────────────────────

    [System.Serializable]
    private class HighscoreEntry
    {
        public string playerName;
        public int    score;
    }

    [System.Serializable]
    private class HighscoreList
    {
        public List<HighscoreEntry> list;
    }
}