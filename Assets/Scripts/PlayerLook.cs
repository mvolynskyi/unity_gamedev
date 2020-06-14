using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] private string mouseXInputName;
    [SerializeField] private string mouseYInputName;
    [SerializeField] private float mouseSensitivity = 150.0f;
    [SerializeField] private Transform playerBody;


    private float xAxisClamp;
    void Awake()
    {
        LockCursor();
        xAxisClamp = 0.0f;
    }
    
    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
    // Update is called once per frame
    void Update()
    {
        CursorLockModeHandler();
        if(Cursor.lockState == CursorLockMode.Locked)
            CameraRotation();
    }

    private void CameraRotation()
    {
        float mouseDPI = mouseSensitivity * Time.deltaTime;

        float mouseX = Input.GetAxis(mouseXInputName) * mouseDPI;
        float mouseY = Input.GetAxis(mouseYInputName) * mouseDPI;

        xAxisClamp += mouseY;
        if(xAxisClamp > 90.0f)
        {
            xAxisClamp = 90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(270.0f);
        }
        else if(xAxisClamp < -90.0f)
        {
            xAxisClamp = -90.0f;
            mouseY = 0.0f;
            ClampXAxisRotationToValue(90.0f);
        }

        transform.Rotate(Vector3.left * mouseY);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void ClampXAxisRotationToValue(float value)
    {
        Vector3 eulerRotation = transform.eulerAngles;
        eulerRotation.x = value;
        transform.eulerAngles = eulerRotation;
    }

    private void CursorLockModeHandler()
    {
        if(Input.GetKeyDown(KeyCode.LeftAlt)/* || Input.GetKeyDown(KeyCode.Escape)*/)
            CursorModeToggle();
    }

    private void CursorModeToggle()
    {
        if(Cursor.lockState == CursorLockMode.Confined || 
            Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }
}
