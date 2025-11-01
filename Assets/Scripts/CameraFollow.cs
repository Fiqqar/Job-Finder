using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float minZ = -20f;
    public float maxZ = -5f;

    void LateUpdate()
    {
        if (target == null) return; // kalo belum ada player skip update

        Vector3 desiredPosition = target.position + offset;

        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
