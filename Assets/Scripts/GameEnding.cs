using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(120);
        SceneManager.LoadScene("EndScene");
    }

    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Playable")
        {
            StartCoroutine(GameTimer());
        }

    }
}