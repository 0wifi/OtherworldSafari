using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public Transform targetPoint;
    public int pointValue;
    [SerializeField] private GameObject sfxPrefab;
    private int stupidCount = 0;

    private void OnEnable()
    {
        stupidCount++;
        if(sfxPrefab != null && stupidCount == 2)
        {
            GameObject.FindAnyObjectByType<AudioManager>().InstantiateSound(sfxPrefab, transform, true);
        }
    }
}
