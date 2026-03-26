using NaughtyAttributes;
using UnityEngine;

public class AnimalController : MonoBehaviour
{
    public Transform targetPoint;
    public int pointValue;
    [SerializeField] private GameObject sfxPrefab;
    private int stupidCount = 0;
    private float lastAnimalXValue;
    private float originalTargetPointX;
    private Vector2 originalAnimalScale;
    [SerializeField][InfoBox("Check if the sprite is facing left.")] private
        bool facingLeft;

    public int timesScored = 0;

    private void OnEnable()
    {
        stupidCount++;
        if(sfxPrefab != null && stupidCount == 2)
        {
            GameObject.FindAnyObjectByType<AudioManager>().InstantiateSound(sfxPrefab, transform, true);
        }
        lastAnimalXValue = gameObject.transform.Find("Animal").position.x;
        originalTargetPointX = targetPoint.localPosition.x;
        originalAnimalScale = transform.Find("Animal").localScale;
    }

    private void Update()
    {
        float currentAnimalXValue = gameObject.transform.Find("Animal").
            position.x;

        /** the fabled nested ternary operator
         * 
        gameObject.transform.Find("Animal").gameObject.GetComponent
            <SpriteRenderer>().flipX = currentAnimalXValue > lastAnimalXValue 
            && facingLeft ? true : currentAnimalXValue < lastAnimalXValue && 
            !facingLeft ? true : false;
        *
        **/

        Transform animalTransform = transform.Find("Animal");

        //flip whole object to respect collider
        if (currentAnimalXValue > lastAnimalXValue && facingLeft)
        {
            animalTransform.localScale = new Vector2(-originalAnimalScale.x, originalAnimalScale.y);
        }
        else if (currentAnimalXValue < lastAnimalXValue && !facingLeft)
        {
            animalTransform.localScale = new Vector2(-originalAnimalScale.x, originalAnimalScale.y);
        }
        else
        {
            animalTransform.localScale = new Vector2(originalAnimalScale.x, originalAnimalScale.y);
        }

        lastAnimalXValue = currentAnimalXValue;
    }
}
