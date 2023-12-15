using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
    [SerializeField] private int _playerLives = 3;

    private void Awake()
    {
        int numberGameSessions = FindObjectsOfType<GameSession>().Length;

        if(numberGameSessions > 1 )
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ProcessPlayerDeath()
    {
        if(_playerLives > 1)
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
        SceneManager.LoadScene(0);
        Destroy(gameObject);
    }

    private void TakeLife()
    {
        _playerLives--;

        int currrentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currrentSceneIndex);
    }
}