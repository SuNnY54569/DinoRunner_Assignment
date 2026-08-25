using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float duration = 0.15f;
    [SerializeField] private float magnitude = 0.15f;
    
    private Vector3 originalPosition;
    private Coroutine shakeCoroutine;

    private float elapsedTime;
    
    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    public void Shake()
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine());
    }

    private IEnumerator ShakeRoutine()
    {
        elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            Vector2 randomOffset = Random.insideUnitCircle * magnitude;

            transform.localPosition = originalPosition +
                                      new Vector3(
                                          randomOffset.x,
                                          randomOffset.y,
                                          0f
                                      );

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}
