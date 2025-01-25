using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager instance;

    public TMP_Text[] scoreTexts;
    public TMP_Text highScoreText;

    int score = 0;
    int highScore = 0;
    string highScoreKey = "HighScore";

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 0; 

        if (PlayerPrefs.HasKey(highScoreKey))
        {
            highScore = PlayerPrefs.GetInt(highScoreKey);
        }

        UpdateScoreText();
        UpdateHighScoreText();
    }
    void UpdateScoreText()
    {
        foreach (TMP_Text scoreDisplay in scoreTexts)
        {
            scoreDisplay.text = score.ToString() + " Score";
        }
    }

    void UpdateHighScoreText()
    {
        highScoreText.text = "High Score: " + highScore.ToString();
    }

    public void AddPoint(int v)
    {
        score += v;
        UpdateScoreText();
        CheckAndUpdateHighScore();
    }

    public void DeductPoint(int v)
    {
        score -= v;
        UpdateScoreText();
        CheckAndUpdateHighScore();
    }
    void CheckAndUpdateHighScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt(highScoreKey, highScore);
            UpdateHighScoreText();
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
}
