using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VisualTimer : MonoBehaviour
{
    [SerializeField] private Slider TimerSlider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(TimerProgressBar());
    }

    IEnumerator TimerProgressBar()
    {
        float currentTime = 0.0f;
        while(true)
        {
            currentTime += Time.deltaTime;
            TimerSlider.value = Mathf.Clamp01(currentTime / 120.0f);
            yield return null;
        }
    }
}
