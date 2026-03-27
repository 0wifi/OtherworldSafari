using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private MotorManager motorManager;
    private InputAction startGame;

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
        SceneManager.LoadScene("Playable");
        motorManager.MotorSound();
    }
}