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

        Destroy(obstacle, destroyIn);
    }
}
