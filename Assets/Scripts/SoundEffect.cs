using System.Collections;
using UnityEngine;

[RequireComponent (typeof(AudioSource))]
public class SoundEffect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public IEnumerator DestroyAfterTime(float seconds)
    {
        yield return new WaitForSeconds (seconds);
        Destroy(gameObject);
    }
}
