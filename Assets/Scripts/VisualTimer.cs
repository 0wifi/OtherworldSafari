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
        while(currentTime < 120.0f)
        {
            yield return new WaitForSeconds(1);
            currentTime++;
            TimerSlider.value = currentTime;
        }
    }
}
