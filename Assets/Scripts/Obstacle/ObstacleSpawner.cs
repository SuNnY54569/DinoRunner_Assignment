using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ObstacleData[] obstacleData;
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn Settings")]
    [SerializeField] private float minSpawnDistance = 10f;
    [SerializeField] private float maxSpawnDistance = 18f;

    private float spawnTimer;
    private float elapsedTime;
    
    private readonly List<Obstacle> spawnedObstacles = new();
    
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

        obstacle.Initialize(gameManager.CurrentSpeed);
        
        spawnedObstacles.Add(obstacle);
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
        float spawnDistance = Random.Range(
            minSpawnDistance,
            maxSpawnDistance
        );

        spawnTimer = spawnDistance / gameManager.CurrentSpeed;
    }
    
    public void ResetSpawner()
    {
        foreach (Obstacle obstacle in spawnedObstacles)
        {
            if (obstacle != null)
            {
                Destroy(obstacle.gameObject);
            }
        }

        spawnedObstacles.Clear();

        elapsedTime = 0f;
        spawnTimer = 0f;

        SetNextSpawnTime();
    }
}
