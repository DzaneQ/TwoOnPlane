using TwoOnPlane.Players;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TwoOnPlane.Players
{
    public class RespawnableAuthoring : MonoBehaviour
    {
        class Baker : Baker<RespawnableAuthoring>
        {
            public override void Bake(RespawnableAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Respawnable());
            }
        }
    }

    public struct Respawnable : IComponentData
    {

    }
}
