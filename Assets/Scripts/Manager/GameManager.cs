using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; private set; }
    
    [Header("Reference")] 
    [SerializeField] private PlayerController playerController;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private ObstacleSpawner obstacleSpawner;
    [SerializeField] private GameOverUI gameOverUI;
    
    [Header("Game Speed")]
    [SerializeField] private float startSpeed = 8f;
    [SerializeField] private float maxSpeed = 15f;
    [SerializeField] private float speedIncrease = 0.5f;
    
    public float CurrentSpeed { get; private set; }
    private float elapsedTime;

    private void Awake()
    {
        CurrentSpeed = startSpeed;
    }

    private void Update()
    {
        if (IsGameOver)
            return;

        elapsedTime += Time.deltaTime;

        CurrentSpeed = Mathf.Min(
            startSpeed + elapsedTime * speedIncrease,
            maxSpeed
        );
    }

    public void GameOver()
    {
        if (IsGameOver)
            return;

        Debug.Log("GameOver");
        IsGameOver = true;
        gameOverUI.ShowGameOver();
        Time.timeScale = 0f;
    }
    
    public void RestartGame()
    {
        Time.timeScale = 1f;

        IsGameOver = false;

        elapsedTime = 0f;
        CurrentSpeed = startSpeed;

        scoreManager.ResetScore();
        playerController.ResetPlayer();
        obstacleSpawner.ResetSpawner();

        gameOverUI.Hide();
    }
}
