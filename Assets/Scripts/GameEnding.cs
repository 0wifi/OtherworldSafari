using NaughtyAttributes;
using System;
using System.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameEnding : MonoBehaviour
{
    public FadeToBlack startFade;
    public FadeToBlack endFade;

    private void Start()
    {
        startFade.StartFade();
    }

    public IEnumerator GameTimer()
    {
        yield return new WaitForSeconds(120);
        StartCoroutine(FadeThenLoad());
    }

    public IEnumerator FadeThenLoad()
    {
        endFade.StartFade();
        yield return new WaitForSeconds(1.2f);
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
        StartCoroutine(FadeThenLoad());
    }
}