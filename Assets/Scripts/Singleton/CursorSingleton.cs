using UnityEngine;

namespace TwoOnPlane.Singleton
{
    public class CursorSingleton : MonoBehaviour
    {
        public static CursorSingleton s_Instance { get; private set; }

        private Camera cam;
        private float _cameraDistance;

        void Awake()
        {
            s_Instance = this;
            cam = Camera.main;
            _cameraDistance = cam.transform.position.y; // Distance between the camera and the plane (for plane y = 0)
        }

        // NOTE: y variable needs to be further adjusted
        public Vector3 GetCursorWorldPosition()
        {
            Vector3 inputPosition = Input.mousePosition;
            inputPosition.z = _cameraDistance;
            return cam.ScreenToWorldPoint(inputPosition);
        }
    }
}
