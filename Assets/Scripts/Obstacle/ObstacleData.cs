using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleData", menuName = "Endless Runner/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    [Header("Basic Info")]
    public string obstacleName;
    public GameObject prefab;

    [Header("Spawn")]
    public float unlockTime;
}
