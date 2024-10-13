using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    Vector3 offset = new Vector3(0, 10, -8);

    void Update()
    {
        transform.position = target.position + offset;
    }
}
