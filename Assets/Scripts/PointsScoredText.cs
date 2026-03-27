using System.Collections;
using UnityEngine;

public class PointsScoredText : MonoBehaviour
{
    public float time = 1;
    void Start()
    {
        StartCoroutine(WaitThenDelete(time));
    }

    public IEnumerator WaitThenDelete(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
