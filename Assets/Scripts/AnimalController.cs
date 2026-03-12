using NaughtyAttributes;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public Transform[] targetPoints;
    public int pointValue;
    [SerializeField] private GameObject sfxPrefab;
    private int stupidCount = 0;
    private float lastAnimalXValue;
    [SerializeField][InfoBox("Check if the sprite is facing left.")] private
        bool facingLeft;

    private void OnEnable()
    {
        stupidCount++;
        if(sfxPrefab != null && stupidCount == 2)
        {
            GameObject.FindAnyObjectByType<AudioManager>().InstantiateSound(sfxPrefab, transform, true);
        }
        lastAnimalXValue = gameObject.transform.Find("Animal").position.x;
    }

    private void Update()
    {
        float currentAnimalXValue = gameObject.transform.Find("Animal").
            position.x;
        gameObject.transform.Find("Animal").gameObject.GetComponent
            <SpriteRenderer>().flipX = currentAnimalXValue > lastAnimalXValue 
            && facingLeft ? true : currentAnimalXValue < lastAnimalXValue && 
            !facingLeft ? true : false;
        lastAnimalXValue = currentAnimalXValue;
    }
}
