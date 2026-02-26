using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFlash : MonoBehaviour
{
    private List<GameObject> flashes; 

    [SerializeField] private GameObject flashPrefab;
    [SerializeField] private float flashDuration;
    [SerializeField] private Vector3 flashOffset; // keeps flashes centered

    private void Start()
    {
        flashes = new List<GameObject>();
    }

    public void doFlash(AccuracyController selectedBox)
    {
        bool flashHappening = false;

        if (flashes.Count > 0)
        {
            foreach (GameObject flash in flashes)
            {
                flashHappening = flash.transform.position == selectedBox.
                    transform.position ? true : flashHappening;
            }
        }

        StartCoroutine(CameraFlashing(selectedBox));
    }

    private IEnumerator CameraFlashing(AccuracyController selectedBox)
    {
        // create flash at selectedbox
        GameObject currentFlash = Instantiate(flashPrefab, selectedBox.
            transform);
        flashes.Add(currentFlash);
        currentFlash.transform.position += flashOffset;

        // make flash fade over time
        float timeElapsed = 0;
        while (timeElapsed < flashDuration)
        {
            Color flashColor = currentFlash.GetComponent<SpriteRenderer>().
                color;
            flashColor.a = Mathf.Lerp(0.3f, 0, timeElapsed / flashDuration);
            currentFlash.GetComponent<SpriteRenderer>().color = flashColor;
            yield return null;
            timeElapsed += Time.deltaTime;
        }
        flashes.Remove(currentFlash);
        Destroy(currentFlash);
        yield return null;
    }
}
