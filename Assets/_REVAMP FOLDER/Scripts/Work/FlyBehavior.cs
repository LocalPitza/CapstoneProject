using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float velocity;

    Rigidbody2D rb;
    private bool isGameOver = false;
    private Vector3 initialPosition;
    private RectTransform rectTransform;

    private void OnEnable()
    {
        // Ensure the GameObject is active
        gameObject.SetActive(true);

        // Initialize the RectTransform when the GameObject is enabled
        rectTransform = GetComponent<RectTransform>();

        // Check if RectTransform is found
        if (rectTransform != null)
        {
            // Set the initial position if it's the first time
            if (initialPosition == Vector3.zero)
            {
                initialPosition = rectTransform.position; // Store the initial position
            }
            rectTransform.position = initialPosition; // Reset position to the initial one
        }
        else
        {
            Debug.LogError("rectTransform is null, cannot reset position.");
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0;
    }

    private void Update()
    {
        // Check if the minigame is active
        if (MiniGameManager.instance != null && !MiniGameManager.instance.IsMiniGameActive())
        {
            return; // Do not allow movement if the minigame is not active
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            rb.velocity = Vector2.up * velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isGameOver) // Prevent multiple calls
        {
            isGameOver = true;

            // Check if collision is with an obstacle
            if (collision.gameObject.CompareTag("MiniGameObstacle"))
            {
                MiniGameManager.instance.GameOver();
            }
        }
    }

    public void DisableGravity()
    {
        if (rb != null)
        {
            rb.gravityScale = 0; // Disable gravity
            rb.velocity = Vector2.zero; // Stop movement
        }
    }

    public void ResetGravity()
    {
        if (rb != null)
        {
            rb.gravityScale = 50; // Re-enable gravity
            rb.velocity = Vector2.zero; // Reset velocity
        }
    }

    public void ResetPosition()
    {
        gameObject.SetActive(true);

        if (rectTransform != null)
        {
            rectTransform.position = initialPosition; // Reset to the recorded initial position for UI elements
        }
        else
        {
            Debug.LogError("rectTransform is null, cannot reset position.");
        }
    }
}
