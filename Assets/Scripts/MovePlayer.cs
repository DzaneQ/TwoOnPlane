using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private const float Speed = 10f;
    private Camera cam;
    private float _cameraDistance;
    private Vector3 _startingPosition;

    void Start()
    {
        cam = Camera.main;
        _cameraDistance = cam.transform.position.y; // Distance between the camera and the plane (for plane y = 0)
        _startingPosition = transform.position;
    }

    void Update()
    {
        MoveTowardsCursor();
        RespawnIfFallen();
    }

    private void MoveTowardsCursor()
    {
        Vector3 inputPosition = Input.mousePosition;
        inputPosition.z = _cameraDistance;
        Vector3 inputWorldPosition = cam.ScreenToWorldPoint(inputPosition);
        Vector3 targetPosition = new(inputWorldPosition.x, transform.position.y, inputWorldPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Speed * Time.deltaTime);
    }

    private void RespawnIfFallen()
    {
        if (transform.position.y > -3) return;
        transform.position = _startingPosition;
    }
}
