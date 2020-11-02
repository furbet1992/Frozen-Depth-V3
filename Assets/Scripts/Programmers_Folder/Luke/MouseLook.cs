/*
    File name: MouseLook.cs
    Author:    Luke Lazzaro
    Summary: Rotates a first person camera with the mouse
    Creation Date: 20/07/2020
    Last Modified: 2/11/2020
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100;
    [SerializeField] private Transform playerBody;

    [Header("Head Bob Settings")]
    [SerializeField] private float amplitude = 1;
    [SerializeField] private float frequency = 0.05f;

    private float xRotation = 0;
    private int sinCounter = 0;

    void Start()
    {
        // Lock cursor by default
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        PlayerMovement pm = playerBody.GetComponent<PlayerMovement>();
        Vector3 playerMove = pm.GetMoveVector();

        if (Vector3.Magnitude(playerMove) > 0.1f && pm.GetGrounded())
        {
            float newY = transform.position.y + Mathf.Sin(frequency * sinCounter) * amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            sinCounter++;
        }

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
