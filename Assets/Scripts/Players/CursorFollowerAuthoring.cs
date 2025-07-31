using TMPro;
using TwoOnPlane.Singleton;
using Unity.Entities;
using Unity.Mathematics;
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

    public struct CursorFollower : IComponentData
    {

    }
}
