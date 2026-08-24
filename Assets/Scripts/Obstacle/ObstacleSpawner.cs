using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ObstacleData[] obstacleData;
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnTime = 1.2f;
    [SerializeField] private float maxSpawnTime = 2.5f;

    [Header("Game Speed")]
    [SerializeField] private float startSpeed = 8f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedIncrease = 0.5f;

    private float spawnTimer;
    private float elapsedTime;
    
    private float CurrentSpeed => Mathf.Min(startSpeed + elapsedTime * speedIncrease, maxSpeed);
    
    private void Start()
    {
        SetNextSpawnTime();
    }
    
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnObstacle();
            SetNextSpawnTime();
        }
    }
    
    private void SpawnObstacle()
    {
        ObstacleData data = GetRandomAvailableObstacle();

        if (data == null)
            return;

        Obstacle obstacle = Instantiate(
            data.prefab,
            spawnPoint.position,
            Quaternion.identity
        ).GetComponent<Obstacle>();

        obstacle.Initialize(CurrentSpeed);
    }
    
    private ObstacleData GetRandomAvailableObstacle()
    {
        List<ObstacleData> availableObstacles = new List<ObstacleData>();

        foreach (ObstacleData data in obstacleData)
        {
            if (elapsedTime >= data.unlockTime)
            {
                availableObstacles.Add(data);
            }
        }

        if (availableObstacles.Count == 0)
            return null;

        int randomIndex = Random.Range(
            0,
            availableObstacles.Count
        );

        return availableObstacles[randomIndex];
    }

    private void SetNextSpawnTime()
    {
        spawnTimer = Random.Range(
            minSpawnTime,
            maxSpawnTime
        );
    }
}
