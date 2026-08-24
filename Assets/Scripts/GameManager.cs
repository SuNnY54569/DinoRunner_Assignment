using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool IsGameOver { get; private set; }
    
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

        IsGameOver = true;
        Time.timeScale = 0f;
    }
}
