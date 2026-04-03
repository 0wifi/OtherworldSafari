using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    public static int score = 0;
    [SerializeField] private TMP_Text scoreText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    /// <summary>
    ///  Adds score to the score text.
    /// </summary>
    /// <param name="points"> number of points to add. </param>
    public void AddScore(int points)
    {
        score += points;
        if (scoreText != null) scoreText.text = "Score: " + score.ToString("N0");
    }

    /// <summary>
    /// Resets the current score to zero.
    /// </summary>
    public void ResetScore()
    {
        score = 0;
    }
}