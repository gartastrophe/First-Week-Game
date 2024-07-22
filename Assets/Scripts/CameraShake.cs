using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float shakeDuration = 1.0f;
    public float shakeMagnitude = 0.2f;

    private Vector3 originalPos;
    private float remainingShakeTime;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (remainingShakeTime > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeMagnitude;
            remainingShakeTime -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = originalPos;
        }
    }

    public void TriggerShake(float duration)
    {
        remainingShakeTime = duration;
    }
}
