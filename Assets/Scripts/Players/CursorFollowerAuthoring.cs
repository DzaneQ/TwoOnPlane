using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace TwoOnPlane.Players
{
    public class CursorFollowerAuthoring : MonoBehaviour
    {
        class Baker : Baker<CursorFollowerAuthoring>
        {
            public override void Bake(CursorFollowerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);
                Camera cam = Camera.main;
                float cameraDistance = cam.transform.position.y;
                Vector3 screenVector = cam.WorldToScreenPoint(authoring.transform.position);

                Debug.Log($"Screen vector: {screenVector.x}, {screenVector.y}");

                AddComponent(entity, new CursorFollower
                {
                    Horizontal = screenVector.x,
                    Vertical = screenVector.y,
                    CameraDistance = cameraDistance
                });
            }
        }
    }

    public struct CursorFollower : IInputComponentData
    {
        public float Horizontal;
        public float Vertical;
        public float CameraDistance;
    }
}
