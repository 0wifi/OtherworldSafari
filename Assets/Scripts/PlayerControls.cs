using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private List<AnimalController> animals;

    [SerializeField] private GameObject pointsScoredTextPrefab;

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
        bool hasHitAnimal = false;
        foreach (AnimalController a in animals)
        {
            float p = selectedBox.GetValuePercentage(a.targetPoint.position);
            int points = (int)(p * a.pointValue);
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
        //TODO: PLAY CAMERA SOUND EFFECT
        if (hasHitAnimal)
        {
            audioManager.PlayRandomOfList(audioManager.CameraSounds, selectedBox.transform, true);
        }
        else
        {
            audioManager.PlaySound(audioManager.CameraMiss, selectedBox.transform, true);
        }

            //TODO: SHOW CAMERA VISUAL EFFECT
            //TODO: START COOLDOWN(?)
        }
    } 
    #region Inputs
    private void BottomRight_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.BottomRight);
    }

    private void BottomMiddle_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.BottomMiddle);
    }

    private void BottomLeft_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.BottomLeft);
    }

    private void CenterRight_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.MiddleRight);
    }

    private void CenterMiddle_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.MiddleCenter);
    }

    private void CenterLeft_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.MiddleLeft);
    }

    private void TopRight_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.TopRight);
    }

    private void TopMiddle_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.TopMiddle);
    }

    private void TopLeft_started(InputAction.CallbackContext obj)
    {
        TakePicture(accuracyManager.TopLeft);
    }
    #endregion
}