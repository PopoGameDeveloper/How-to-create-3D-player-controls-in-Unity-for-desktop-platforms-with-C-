// Import necessary Unity libraries
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Define a new class called KeyboardMovement that inherits from MonoBehaviour
public class KeyboardMovement : MonoBehaviour
{
    // Define public variables for move speed, jump force, and rotation speed
    public float moveSpeed = 5f; // Speed at which the character moves
    public float jumpForce = 10f; // Force applied to the character when jumping
    public float rotationSpeed = 5f; // Speed at which the character rotates

    // Define private variables for Rigidbody, grounded state, Camera, and rotation values
    private Rigidbody rb; // Rigidbody component attached to the character
    private bool isGrounded = true; // Whether the character is grounded or not
    private Camera cam; // Camera component attached to the character
    private Vector3 originalCameraRotation; // Original rotation of the camera
    private float rotationY = 0f; // Rotation around the Y axis
    private float rotationX = 0f; // Rotation around the X axis

    // Called once at the start of the game
    void Start()
    {
        // Get the Rigidbody component attached to the character
        rb = GetComponent<Rigidbody>();
        
        // Get the Camera component attached to the character
        cam = GetComponentInChildren<Camera>(); // get camera
        
        // Store the original rotation of the camera
        originalCameraRotation = cam.transform.localEulerAngles; // Save original camera rotation
    }

    // Called every frame
    void Update()
    {
        // Get input from the keyboard
        float horizontalInput = Input.GetAxis("Horizontal") + (Input.GetKey(KeyCode.A) ? -1 : 0) + (Input.GetKey(KeyCode.D) ? 1 : 0);
        float verticalInput = Input.GetAxis("Vertical") + (Input.GetKey(KeyCode.W) ? 1 : 0) + (Input.GetKey(KeyCode.S) ? -1 : 0);

        // Rotate the camera with the mouse
        GameObject canvasObject = GameObject.Find("Canvas");
        RectTransform canvasRectTransform = canvasObject.GetComponent<RectTransform>();

        Vector2 mousePosition = Input.mousePosition;
        if (RectTransformUtility.RectangleContainsScreenPoint(canvasRectTransform, mousePosition))
        {
            // Calculate mouse movement
            float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
            float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;

            // Update rotation values
            rotationY += mouseX; // Horizontal Rotation
            rotationX -= mouseY; // Vertical Rotation

            // Limit rotation values
            rotationX = Mathf.Clamp(rotationX, -45f, 45f);

            // Apply rotation to the camera
            // cam.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

            // Rotate the character
            transform.rotation = Quaternion.Euler(25f, rotationY, 0f);
        }

        // Calculate movement vector
        // Calculate movement vector
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed;
        movement = transform.TransformDirection(movement); // Rotate the movement vector by the character's rotation

        // Apply movement
        rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

        // Check for jump input
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Call the Jump method
            Jump();
        }
    }

    // Method to handle jumping
    void Jump()
    {
        // Apply a force upward to the character
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    // Called when the character collides with another object
    void OnCollisionEnter(Collision collision)
    {
        // Check if the collided object is tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Set the grounded state to true
            isGrounded = true;
        }
    }

    // Called when the character exits a collision with another object
    void OnCollisionExit(Collision collision)
    {
        // Check if the exited object is tagged as "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Set the grounded state to false
            isGrounded = false;
        }
    }
}