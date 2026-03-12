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

    //[SerializeField] private List<AnimalController> animals; **** now found automatically in TakePicture()
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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

    private void TakePicture(AccuracyController selectedBox)
    {
        if (!isCameraOnCooldown)
        {
            bool hasHitAnimal = false;

            //find all active animals in animal control object
            List<AnimalController> animals = animalControlObject.GetComponentsInChildren<AnimalController>().ToList<AnimalController>();

            //print(animals.Count + " animals found.");

            foreach (AnimalController a in animals)
            {
                //get point percentage value from accuracy box
                float p = selectedBox.GetValuePercentage(a.targetPoint.position);
                int points = (int)(p * a.pointValue);

                // if scored
                if (p > 0)
                {
                    hasHitAnimal = true;
                    //spawn scoretext object
                    GameObject canvas = GameObject.FindFirstObjectByType<Canvas>().gameObject;
                    GameObject instance = Instantiate(pointsScoredTextPrefab, Camera.main.WorldToScreenPoint(a.targetPoint.position), Quaternion.identity, canvas.transform);
                    instance.GetComponent<TMP_Text>().text = points.ToString();

                    Debug.Log("Points: " + points);
                    scoreManager.AddScore(points);
                }
            }

            if (hasHitAnimal) //HAS HIT AN ANIMAL
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
            else //HAS MISSED
            {
                //play camera miss sound
                audioManager.InstantiateSound(audioManager.CameraMiss, selectedBox.transform, true);
            }

        }
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
        TakePicture(accuracyManager.BottomRight);
        ButtonPressCountJSONService.pressCounts.BOTTOM_RIGHT_COUNT++;
    }

    private void BottomMiddle_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.BottomMiddle);
        ButtonPressCountJSONService.pressCounts.BOTTOM_MIDDLE_COUNT++;
    }

    private void BottomLeft_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.BottomLeft);
        ButtonPressCountJSONService.pressCounts.BOTTOM_LEFT_COUNT++;
    }

    private void CenterRight_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.MiddleRight);
        ButtonPressCountJSONService.pressCounts.MIDDLE_RIGHT_COUNT++;
    }

    private void CenterMiddle_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.MiddleCenter);
        ButtonPressCountJSONService.pressCounts.CENTER_COUNT++;
    }

    private void CenterLeft_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.MiddleLeft);
        ButtonPressCountJSONService.pressCounts.MIDDLE_LEFT_COUNT++;
    }

    private void TopRight_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.TopRight);
        ButtonPressCountJSONService.pressCounts.TOP_RIGHT_COUNT++;
    }

    private void TopMiddle_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.TopMiddle);
        ButtonPressCountJSONService.pressCounts.TOP_MIDDLE_COUNT++;
    }

    private void TopLeft_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.TopLeft);
        ButtonPressCountJSONService.pressCounts.TOP_LEFT_COUNT++;
    }
    #endregion
}