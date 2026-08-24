using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCleanUp : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out Obstacle obstacle))
        {
            Destroy(obstacle.gameObject);
        }
    }
}
