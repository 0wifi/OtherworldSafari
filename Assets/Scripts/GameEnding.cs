using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
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