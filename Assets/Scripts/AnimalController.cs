using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public Transform targetPoint;
    public int pointValue;
    [SerializeField] private GameObject sfxPrefab;

    private void OnEnable()
    {
        if(sfxPrefab != null)
        {
            GameObject.FindAnyObjectByType<AudioManager>().InstantiateSound(sfxPrefab, transform, true);
        }
    }
}
