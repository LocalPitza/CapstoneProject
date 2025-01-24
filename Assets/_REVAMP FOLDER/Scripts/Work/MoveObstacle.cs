using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    [SerializeField] float obstacleSpeed;

    private void Update()
    {
        // Check if the minigame is active before updating the position
        if (MiniGameManager.instance != null && !MiniGameManager.instance.IsMiniGameActive())
        {
            return; // Stop movement if the minigame is not active
        }

        transform.position += Vector3.left * obstacleSpeed * Time.deltaTime;
    }
}
