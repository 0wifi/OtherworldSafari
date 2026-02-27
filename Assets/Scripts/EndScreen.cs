using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class EndScreen : MonoBehaviour
{
    private ScoreManager scoreManager;
    public TMP_Text scoreText;
    [SerializeField] private PlayerInput playerInput;
    private InputAction returnToStart;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        playerInput.currentActionMap.Enable();

        returnToStart = playerInput.currentActionMap.FindAction("ReturnToStart");

        returnToStart.started += ReturnToStart_started;
    }

    private void ReturnToStart_started(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene("StartScene");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (scoreText != null) scoreText.text = "Score: " + ScoreManager.score;
    }
}