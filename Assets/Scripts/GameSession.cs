using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    private const string _livesTextTemplate = "Lives: ";
    private const string _scoreTextTemplate = "Score: ";

    [SerializeField] private int _playerLives = 10;
    [SerializeField] private int _score = 0;

    [SerializeField] private TextMeshProUGUI _livesText;
    [SerializeField] private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;

        if (numberGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        _livesText.text = _livesTextTemplate + _playerLives.ToString();
        _scoreText.text = _scoreTextTemplate + _score.ToString();
    }

    public void AddScore(int pointsToAdd)
    {
        _score += pointsToAdd;
        _scoreText.text = _scoreTextTemplate + _score.ToString();
    }

    public void ProcessPlayerDeath()
    {
        if (_playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    private void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        _playerLives--;

        int currrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currrentSceneIndex);

        _livesText.text = _livesTextTemplate + _playerLives.ToString();
    }
}