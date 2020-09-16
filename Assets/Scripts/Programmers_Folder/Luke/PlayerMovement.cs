/*
    File name: PlayerMovement.cs
    Author:    Luke Lazzaro
    Summary: Adds first person movement to the player
    Creation Date: 20/07/2020
    Last Modified: 15/09/2020
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private float speed = 12;
    [SerializeField] private float crouchSpeed = 6;
    public float gravity = -9.81f;
    [SerializeField] private float jumpHeight = 3;
    [Tooltip("The position of this transform determines where the player touches the ground.")]
    [SerializeField] private Transform groundCheck;
    [Tooltip("The layer for all objects you can walk on.")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckRadius = 0.6f;

    [Header("Camera")]
    [SerializeField] private GameObject playerCamera;
    [SerializeField] private float camHeight = 2;
    [SerializeField] private float camCrouchHeight = 1;

    [Header("Char. Controller")]
    [SerializeField] private float ccHeight = 3;
    [SerializeField] private float ccCrouchHeight = 2;
    [SerializeField] private float deathVelocity = -20;

    [HideInInspector] public bool willDie = false;

    private CharacterController controller;
    
    private Vector3 velocity;
    private bool isGrounded;
    private bool isCrouching = true;
    private Vector3 originalPos;

    // used to store the distance between controller.center and the halfway point on the collider
    private float standCenterHeight = 0f;

    private void Start()
    {
        originalPos = transform.position;
        controller = GetComponent<CharacterController>();
        standCenterHeight = (ccHeight - ccCrouchHeight) * 0.5f;
        Crouch();
    }

    private void Update()
    {
        Vector3 temp = new Vector3(controller.center.x, controller.height - controller.center.y, controller.center.z);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);

        if (isGrounded && velocity.y < deathVelocity)
        {
            willDie = true;
        }

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        if (isCrouching)
            controller.Move(move * crouchSpeed * Time.deltaTime);
        else
            controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void LateUpdate()
    {
        if (willDie)
        {
            GoToLastCheckpoint();
            willDie = false;
        }
    }

    private void Crouch()
    {
        if (isCrouching)
        {
            if (Physics.Raycast(transform.position, Vector3.up, standCenterHeight + (ccHeight / 2)))
                return;
        }

        isCrouching = !isCrouching;        

        if (isCrouching)
        {
            controller.height = ccCrouchHeight;
            Vector3 newCenter = new Vector3(controller.center.x, (controller.center.y - (ccHeight - ccCrouchHeight) * 0.5f), controller.center.z);
            controller.center = newCenter;

            float camX = playerCamera.transform.localPosition.x;
            float camZ = playerCamera.transform.localPosition.z;
            Vector3 newCamPos = new Vector3(camX, camCrouchHeight, camZ);
            playerCamera.transform.localPosition = newCamPos;
        }
        else
        {
            controller.height = ccHeight;
            Vector3 newCenter = new Vector3(controller.center.x, (ccHeight - ccCrouchHeight) * 0.5f, controller.center.z);
            controller.center = newCenter;

            float camX = playerCamera.transform.localPosition.x;
            float camZ = playerCamera.transform.localPosition.z;
            Vector3 newCamPos = new Vector3(camX, camHeight, camZ);
            playerCamera.transform.localPosition = newCamPos;
        }

        float gcX = groundCheck.localPosition.x;
        float gcZ = groundCheck.localPosition.z;
        Vector3 newPos = new Vector3(gcX, controller.center.y - controller.height * 0.5f, gcZ);
        groundCheck.localPosition = newPos;
    }

    // Call this in LateUpdate, otherwise the position will be overwritten by player movement.
    private void GoToLastCheckpoint()
    {
        if (CheckpointManager.currentCheckpoint != null)
        {
            transform.position = CheckpointManager.currentCheckpoint.transform.position;
        }
        else
        {
            transform.position = originalPos;
        }
    }
}
