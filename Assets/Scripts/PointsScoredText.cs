using System.Collections;
using UnityEngine;

public class PointsScoredText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(WaitThenDelete(1.5f));
    }

    public IEnumerator WaitThenDelete(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
