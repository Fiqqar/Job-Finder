using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public LevelGenerator levelGenerator;
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float minZ = -20f;
    public float maxZ = -5f;

    private float shakeTimer = 0f;
    private float shakeStrength = 0f;
    private Vector3 originalPos;

    void Start()
    {
        originalPos = transform.localPosition;
    }
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        if (shakeTimer > 0)
        {
            transform.localPosition += (Vector3)(Random.insideUnitCircle * shakeStrength);
            shakeTimer -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
    }

    public void Shake(float duration, float strength)
    {
        shakeTimer = duration;
        shakeStrength = strength;
    }
    public void StopShake()
    {
        shakeTimer = 0f;
        transform.localPosition = originalPos;
    }
}
