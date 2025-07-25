using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    private const float Speed = 1f;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        Vector3 playerScreenPosition = cam.WorldToScreenPoint(transform.position);
        Vector3 inputPosition = Input.mousePosition;
        //Vector3 inputWorldPosition = cam.ScreenToWorldPoint(inputPosition);
        //Debug.Log($"Input position: {inputWorldPosition.x}, {inputWorldPosition.y}, {inputWorldPosition.z}");
        Debug.Log($"Player position: {transform.position.x}, {transform.position.y}, {transform.position.z}");
        //Vector3 targetPosition = new(inputWorldPosition.x, transform.position.y, inputWorldPosition.z);
        //transform.position = cam.ScreenToWorldPoint(Vector3.MoveTowards(cam.ScreenToWorldPoint(playerScreenPosition), targetPosition, Speed * Time.deltaTime));
    }
}
