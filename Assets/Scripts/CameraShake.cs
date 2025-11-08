using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPos;
    private float shakeTimer = 0f;
    private float shakeStrength = 0f;

    void Start()
    {
        originalPos = transform.localPosition;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            transform.localPosition = originalPos + Random.insideUnitSphere * shakeStrength;
            shakeTimer -= Time.deltaTime;

            if (shakeTimer <= 0)
                transform.localPosition = originalPos;
        }
    }

    public void Shake(float duration, float strength)
    {
        shakeTimer = duration;
        shakeStrength = strength;
    }
}
