using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowGameOver()
    {
        if (gameOverPanel.activeSelf)
            return;

        scoreText.text = $"SCORE: {scoreManager.CurrentScore:0000}";
        highScoreText.text = $"HIGH SCORE: {scoreManager.HighScore:0000}";

        gameOverPanel.SetActive(true);
    }
    
    public void Hide()
    {
        gameOverPanel.SetActive(false);
    }
}
