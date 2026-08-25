using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highScoreText;

    private void Update()
    {
        scoreText.text = $"SCORE: {scoreManager.CurrentScore:0000}";
        highScoreText.text = $"HIGH SCORE: {scoreManager.HighScore:0000}";
    }
    
    public void SetScoreActive(bool isActive)
    {
        scoreText.gameObject.SetActive(isActive);
        highScoreText.gameObject.SetActive(isActive);
    }
}
