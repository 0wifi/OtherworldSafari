using System.Collections;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public int time = 1;

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
