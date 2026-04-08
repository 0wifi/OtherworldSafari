using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MotorManager motorManager;
    private InputAction startGame;

    public FadeToBlack fadeToBlack;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput.currentActionMap.Enable();

        startGame = playerInput.currentActionMap.FindAction("StartGame");

        startGame.started += StartGame_started;

        Cursor.visible = false;

        //scoreManager.ResetScore();
    }

    private void StartGame_started(InputAction.CallbackContext obj)
    {
        motorManager.MotorSound();
        StartCoroutine(FadeThenLoad());
    }
    
    public IEnumerator FadeThenLoad()
    {
        fadeToBlack.StartFade();
        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("Playable");
    }
}