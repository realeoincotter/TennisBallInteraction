using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject gameOverText;   // a TMP text on your scoreboard canvas
    public float restartDelay = 5f;

    public bool IsGameOver { get; private set; }

    void Awake()
    {
        Instance = this;
        if (gameOverText) gameOverText.SetActive(false);
    }

    public void EndGame()
    {
        if (IsGameOver) return;
        IsGameOver = true;

        if (gameOverText) gameOverText.SetActive(true);
        Debug.Log("Game over! Final score: " + ScoreManager.Instance.Score);

        Invoke(nameof(Restart), restartDelay);
    }

    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}