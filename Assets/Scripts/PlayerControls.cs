using NaughtyAttributes;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerControls : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction topLeft;
    private InputAction topMiddle;
    private InputAction topRight;
    private InputAction centerLeft;
    private InputAction centerMiddle;
    private InputAction centerRight;
    private InputAction bottomLeft;
    private InputAction bottomMiddle;
    private InputAction bottomRight;

    private ScoreManager scoreManager;

    [SerializeField] private AccuracyManager accuracyManager;

    [SerializeField] private AudioManager audioManager;

    [Required]
    [SerializeField] GameObject animalControlObject;

    [SerializeField] private GameObject pointsScoredTextPrefab;

    [SerializeField] private ImageCapture imageCapture;

    [SerializeField] private CameraFlash cameraFlash;

    private bool isCameraOnCooldown = false;

    public float CooldownTime = 0.1f;

    [Tooltip("Cooldown for saving image to display on end screen")]
    public float ImageSaveCooldown = 6f;
    private bool shouldSaveImage = true;

    public LayerMask cameraCollisionLayerMask;

    [Tooltip("Percentage of animal's value that is added for each photo taken")]
    public float comboScoreModifier = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        scoreManager = GameObject.FindFirstObjectByType<ScoreManager>();

        if (TryGetComponent<PlayerInput>(out playerInput))
        {
            playerInput.currentActionMap.Enable();

            topLeft = playerInput.currentActionMap.FindAction("TopLeft");
            topMiddle = playerInput.currentActionMap.FindAction("TopMiddle");
            topRight = playerInput.currentActionMap.FindAction("TopRight");
            centerLeft = playerInput.currentActionMap.FindAction("CenterLeft");
            centerMiddle = playerInput.currentActionMap.FindAction("CenterMiddle");
            centerRight = playerInput.currentActionMap.FindAction("CenterRight");
            bottomLeft = playerInput.currentActionMap.FindAction("BottomLeft");
            bottomMiddle = playerInput.currentActionMap.FindAction("BottomMiddle");
            bottomRight = playerInput.currentActionMap.FindAction("BottomRight");

            topLeft.started += TopLeft_started;
            topMiddle.started += TopMiddle_started;
            topRight.started += TopRight_started;
            centerLeft.started += CenterLeft_started;
            centerMiddle.started += CenterMiddle_started;
            centerRight.started += CenterRight_started;
            bottomLeft.started += BottomLeft_started;
            bottomMiddle.started += BottomMiddle_started;
            bottomRight.started += BottomRight_started;
        }

        if (accuracyManager == null)
        {
            throw new System.Exception("AccuracyManager not assigned");
        }
    }

    /// <summary>
    ///     Checks for collisions of animals in the specified accuracy box and processes the results as either hits or a miss.
    /// </summary>
    /// <param name="selectedBox">Selected grid space's accuracy box</param>
    private void TakePictureCollision(AccuracyController selectedBox)
    {
        if (isCameraOnCooldown) return; //ensure camera not on cooldown

        //contact filter for OverlapBox
        ContactFilter2D contactFilter = ContactFilter2D.noFilter;
        contactFilter.layerMask = cameraCollisionLayerMask;

        Rect gridSpaceRect = selectedBox.GetBoundingRect(); //bounding rect of grid space

        List<Collider2D> collisions = new List<Collider2D>(); //list for contact results

        //if output contacts > 0 (hit at least one animal)
        if (Physics2D.OverlapBox(selectedBox.transform.position, gridSpaceRect.size, 0.0f, contactFilter, collisions) > 0) 
        {
            //score each animal detected
            foreach (Collider2D collider in collisions)
            {
                AnimalController animal = collider.transform.parent.GetComponent<AnimalController>();
                ScoreAnimal(animal);
            }

            HasHitAnimal(selectedBox);
        }
        else { HasMissedAnimal(selectedBox); }
    }

    /// <summary>
    ///     Scores an animal based on its' point value, adding to the scoremanager and displaying the point text.
    /// </summary>
    /// <param name="animal">Animal to be scored</param>
    private void ScoreAnimal(AnimalController animal)
    {
        //spawn scoretext object
        GameObject canvas = GameObject.FindFirstObjectByType<Canvas>().gameObject;
        GameObject instance = Instantiate(pointsScoredTextPrefab, Camera.main.WorldToScreenPoint(animal.targetPoint.position), Quaternion.identity, canvas.transform);

        int points = (int)(animal.pointValue * (1 + (comboScoreModifier * animal.timesScored)));
        animal.timesScored++;
        
        TMP_Text scoreText = instance.GetComponentInChildren<TMP_Text>();
        scoreText.text = points.ToString(); //give score text point value
        scoreText.color = Color.Lerp(Color.white, Color.goldenRod, Mathf.Clamp01((float)animal.timesScored / 20)); //change color based on how many photos taken

        //add to score
        scoreManager.AddScore(points);
    }

    /// <summary>
    ///     Handles the actions to perform when at least one animal is detected in an attempted photo.
    /// </summary>
    /// <param name="selectedBox">Selected grid space's accuracy box</param>
    private void HasHitAnimal(AccuracyController selectedBox)
    {
        //play camera hit sound
        audioManager.InstantiateRandomOfList(audioManager.CameraHit, selectedBox.transform, true);

        //get image capture from image capture system, saving if should save image
        if (shouldSaveImage)
        {
            imageCapture.StartCoroutine(imageCapture.CaptureImageAndSaveSprite(selectedBox.GetCamera()));
            StartCoroutine(SaveSpriteCooldown(ImageSaveCooldown));
        }
        else
        {
            imageCapture.StartCoroutine(imageCapture.CaptureImage(selectedBox.GetCamera()));
        }

        //show camera visual effect
        cameraFlash.doFlash(selectedBox);

        StartCoroutine(CameraCooldown(CooldownTime));
    }

    /// <summary>
    ///     Handles actions to perform when no animals are detected in an attempted photo.
    /// </summary>
    /// <param name="selectedBox">Selected grid space's accuracy box</param>
    private void HasMissedAnimal(AccuracyController selectedBox)
    {
        //play camera miss sound
        audioManager.InstantiateSound(audioManager.CameraMiss, selectedBox.transform, true);
    }

    public IEnumerator CameraCooldown(float seconds)
    {
        isCameraOnCooldown = true;
        yield return new WaitForSeconds(seconds);
        isCameraOnCooldown = false;
    }

    public IEnumerator SaveSpriteCooldown(float seconds)
    {
        shouldSaveImage = false;
        yield return new WaitForSeconds(seconds);
        shouldSaveImage = true;
    }

    #region Inputs
    private void BottomRight_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.BottomRight);
        ButtonPressCountJSONService.pressCounts.BOTTOM_RIGHT_COUNT++;
    }

    private void BottomMiddle_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.BottomMiddle);
        ButtonPressCountJSONService.pressCounts.BOTTOM_MIDDLE_COUNT++;
    }

    private void BottomLeft_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.BottomLeft);
        ButtonPressCountJSONService.pressCounts.BOTTOM_LEFT_COUNT++;
    }

    private void CenterRight_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.MiddleRight);
        ButtonPressCountJSONService.pressCounts.MIDDLE_RIGHT_COUNT++;
    }

    private void CenterMiddle_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.MiddleCenter);
        ButtonPressCountJSONService.pressCounts.CENTER_COUNT++;
    }

    private void CenterLeft_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.MiddleLeft);
        ButtonPressCountJSONService.pressCounts.MIDDLE_LEFT_COUNT++;
    }

    private void TopRight_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.TopRight);
        ButtonPressCountJSONService.pressCounts.TOP_RIGHT_COUNT++;
    }

    private void TopMiddle_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.TopMiddle);
        ButtonPressCountJSONService.pressCounts.TOP_MIDDLE_COUNT++;
    }

    private void TopLeft_started(InputAction.CallbackContext obj)
    {
        TakePictureCollision(accuracyManager.TopLeft);
        ButtonPressCountJSONService.pressCounts.TOP_LEFT_COUNT++;
    }
    #endregion

    private void OnDestroy()
    {
        topLeft.started -= TopLeft_started;
        topMiddle.started -= TopMiddle_started;
        topRight.started -= TopRight_started;
        centerLeft.started -= CenterLeft_started;
        centerMiddle.started -= CenterMiddle_started;
        centerRight.started -= CenterRight_started;
        bottomLeft.started -= BottomLeft_started;
        bottomMiddle.started -= BottomMiddle_started;
        bottomRight.started -= BottomRight_started;
    }
}