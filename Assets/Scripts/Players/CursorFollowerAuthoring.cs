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

                AddComponent(entity, new CursorFollower
                {
                    Horizontal = authoring.transform.position.x,
                    Vertical = authoring.transform.position.z,
                });
            }
        }
    }

    public struct CursorFollower : IInputComponentData
    {
        public float Horizontal;
        public float Vertical;
    }
}
