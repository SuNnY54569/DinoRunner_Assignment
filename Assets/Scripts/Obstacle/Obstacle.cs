using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float moveSpeed;

    public void Initialize(float speed)
    {
        moveSpeed = speed;
    }

    private void Update()
    {
        transform.Translate(Vector2.left * (moveSpeed * Time.deltaTime));
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
