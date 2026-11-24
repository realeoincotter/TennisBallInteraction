using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TMP_Text scoreText;     // shows "Score: 1250"
    public TMP_Text highScoreText; // shows "Best: 3000" (optional)
    public TMP_Text hitText;       // pops up "+500!" (optional)

    public int Score { get; private set; }
    public int HighScore { get; private set; }

    void Awake()
    {
        Instance = this;
        HighScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
        if (hitText) hitText.text = "";
    }

    public void AddPoints(int points)
    {
        Score += points;

        if (Score > HighScore)
        {
            HighScore = Score;
            PlayerPrefs.SetInt("HighScore", HighScore);
        }

        UpdateUI();
        if (hitText) StartCoroutine(ShowHit(points));
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (scoreText) scoreText.text = $"Score: {Score}";
        if (highScoreText) highScoreText.text = $"Best: {HighScore}";
    }

    IEnumerator ShowHit(int points)
    {
        hitText.text = points == 500 ? "BULLSEYE! +500" : $"+{points}";
        yield return new WaitForSeconds(1f);
        hitText.text = "";
    }
}