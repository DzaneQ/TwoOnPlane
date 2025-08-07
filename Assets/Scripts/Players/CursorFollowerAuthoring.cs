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

                AddComponent(entity, new CursorFollower());
            }
        }
    }

    public struct CursorFollower : IInputComponentData
    {
        [GhostField] public float Horizontal;
        [GhostField] public float Vertical;
        [GhostField] public bool IsMoving;
    }
}
