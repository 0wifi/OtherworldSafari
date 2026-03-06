using NaughtyAttributes;
using System;
using System.Collections;
using Unity.Hierarchy;
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

    [Button]
    private void SkipToEnd()
    {
        SceneManager.LoadScene("EndScene");
    }
}