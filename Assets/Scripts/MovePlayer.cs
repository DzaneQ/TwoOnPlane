using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private const float Speed = 10f;
    private Camera cam;
    private float _cameraDistance;

    void Start()
    {
        cam = Camera.main;
        _cameraDistance = cam.transform.position.y; // Distance between the camera and the plane (for plane y = 0)
    }

    void Update()
    {
        MoveTowardsCursor();
    }

    private void MoveTowardsCursor()
    {
        Vector3 inputPosition = Input.mousePosition;
        inputPosition.z = _cameraDistance;
        Vector3 inputWorldPosition = cam.ScreenToWorldPoint(inputPosition);
        Vector3 targetPosition = new(inputWorldPosition.x, transform.position.y, inputWorldPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
    }
}
