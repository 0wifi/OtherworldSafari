using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreen : MonoBehaviour
{
    private ScoreManager scoreManager;
    public TMP_Text scoreText;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scoreText != null) scoreText.text = "Score: " + ScoreManager.score;
    }
}