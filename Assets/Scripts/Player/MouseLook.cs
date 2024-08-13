using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public Transform playerBody;

    public float mouseSensitivity = 10f;

    float pitch = 0f;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Yaw
        playerBody.Rotate(Vector3.up * moveX);

        // Pitch
        pitch -= moveY;
        pitch = Mathf.Clamp(pitch, -90f, 90f);

        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
