using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;          // player di hierarchy
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -10); // default aman untuk Perspective
    public float minZ = -20f;         // clamp minimum z
    public float maxZ = -5f;          // clamp maksimum z

    void LateUpdate()
    {
        if (target == null) return; // kalo belum ada player, skip update

        Vector3 desiredPosition = target.position + offset;

        // clamp z supaya camera ga terlalu dekat atau jauh
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, minZ, maxZ);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
