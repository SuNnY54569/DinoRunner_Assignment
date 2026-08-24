using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [Header("Reference")] 
    [SerializeField] private GameManager gameManager;
    
    [Header("Score")]
    [SerializeField] private float scoreMultiplier = 1f;

    public int CurrentScore { get; private set; }
    public int HighScore { get; private set; }

    private float distance;

    private void Update()
    {
        distance += Time.deltaTime * gameManager.CurrentSpeed;
        CurrentScore = Mathf.FloorToInt(distance * scoreMultiplier);

        if (CurrentScore > HighScore)
        {
            HighScore = CurrentScore;
        }
        
        Debug.Log($"Score = {CurrentScore}");
    }
}
