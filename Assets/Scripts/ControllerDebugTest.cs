using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ControllerDebugTest : MonoBehaviour
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

    public Image[] images = new Image[9];


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

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

        for(int i = 0; i < 10; i++)
        {
            images[i].color = Color.darkRed;
        }
    }

    private void BottomRight_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(8));
    }

    private void BottomMiddle_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(7));
    }

    private void BottomLeft_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(6));
    }

    private void CenterRight_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(5));
    }

    private void CenterMiddle_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(4));
    }

    private void CenterLeft_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(3));
    }

    private void TopRight_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(2));
    }

    private void TopMiddle_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(1));
    }

    private void TopLeft_started(InputAction.CallbackContext obj)
    {
        StartCoroutine(Test(0));
    }

    public IEnumerator Test(int i)
    {
        images[i].color = Color.darkGreen;
        yield return new WaitForSeconds(.5f);
        images[i].color = Color.white;
    }
}
