using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBehavior : MonoBehaviour
{
    [SerializeField] private float velocity;

    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        MiniGameManager.instance.GameOver();
    }
}
