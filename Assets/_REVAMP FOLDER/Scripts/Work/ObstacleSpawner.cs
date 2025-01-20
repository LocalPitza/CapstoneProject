using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Next Obstacle in")]
    [SerializeField] float maxTime;

    [Header("Up/Down of Y-axis")]
    [SerializeField] float heightRange;

    [Header("Self Destruct")]
    [SerializeField] float destroyIn;

    [Header("Obstacle Prefab")]
    [SerializeField] GameObject obstalce;

    [Header("Obstacle SpawnPoint Canvas")]
    [SerializeField] RectTransform obstacleSpawnPoint;

    float timer;
    private List<GameObject> spawnedObstacles = new List<GameObject>();

    private void Start()
    {
        SpawnObstacle();
    }

    private void Update()
    {
        if (!MiniGameManager.instance.IsMiniGameActive())
        {
            return; // Stop execution if the minigame is not active
        }

        if (timer > maxTime)
        {
            SpawnObstacle();
            timer = 0;
        }

        timer += Time.deltaTime;
    }

    private void SpawnObstacle()
    {
        // Calculate spawn position within the Canvas' local space
        float randomHeight = Random.Range(-heightRange, heightRange);
        Vector2 spawnPos = new Vector2(0, randomHeight);

        // Instantiate the UI obstacle and set it as a child of the Canvas
        GameObject obstacle = Instantiate(obstalce, obstacleSpawnPoint);
        RectTransform rectTransform = obstacle.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = spawnPos; // Set position relative to the Canvas
        }

        spawnedObstacles.Add(obstacle);

        Destroy(obstacle, destroyIn);
    }

    public void ResetSpawner()
    {
        // Debug to check if the method is being called
        Debug.Log("ResetSpawner called");

        // Stop any existing obstacle spawning and destroy all spawned obstacles
        foreach (GameObject obstacle in spawnedObstacles)
        {
            Destroy(obstacle); // Destroy existing obstacles
        }
        spawnedObstacles.Clear(); // Clear the list of obstacles

        // Restart obstacle spawning by resetting the timer
        timer = maxTime;
    }
}
