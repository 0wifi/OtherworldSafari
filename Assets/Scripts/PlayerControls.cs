using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    private PlayerInput playerInput;
    public InputAction topLeft;
    public InputAction topMiddle;
    public InputAction topRight;
    public InputAction centerLeft;
    public InputAction centerMiddle;
    public InputAction centerRight;
    public InputAction bottomLeft;
    public InputAction bottomMiddle;
    public InputAction bottomRight;

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
    }

    private void BottomRight_started(InputAction.CallbackContext obj)
    {
        
    }

    private void BottomMiddle_started(InputAction.CallbackContext obj)
    {
        
    }

    private void BottomLeft_started(InputAction.CallbackContext obj)
    {
        
    }

    private void CenterRight_started(InputAction.CallbackContext obj)
    {
        
    }

    private void CenterMiddle_started(InputAction.CallbackContext obj)
    {
        
    }

    private void CenterLeft_started(InputAction.CallbackContext obj)
    {
        
    }

    private void TopRight_started(InputAction.CallbackContext obj)
    {
        
    }

    private void TopMiddle_started(InputAction.CallbackContext obj)
    {
        
    }

    private void TopLeft_started(InputAction.CallbackContext obj)
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}