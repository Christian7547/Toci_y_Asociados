using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset = new Vector3(0, 15, -2);

    void Update()
    {
        transform.position = target.position + offset;
    }
}
