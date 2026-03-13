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
        originalTargetPointX = targetPoint.localPosition.x;
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

        SpriteRenderer sr = gameObject.transform.Find("Animal").gameObject.GetComponent<SpriteRenderer>();

        if (currentAnimalXValue > lastAnimalXValue && facingLeft)
        {
            sr.flipX = true;
            targetPoint.localPosition = new Vector2(-originalTargetPointX, targetPoint.localPosition.y);
        }
        else if (currentAnimalXValue < lastAnimalXValue && !facingLeft)
        {
            sr.flipX = true;
            targetPoint.localPosition = new Vector2(-originalTargetPointX, targetPoint.localPosition.y);
        }
        else
        {
            sr.flipX = false;
            targetPoint.localPosition = new Vector2(originalTargetPointX, targetPoint.localPosition.y);
        }

        lastAnimalXValue = currentAnimalXValue;
    }
}
