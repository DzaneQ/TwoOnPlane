using TwoOnPlane.Player;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TwoOnPlane.Player
{
    public class RespawnableAuthoring : MonoBehaviour
    {
        class Baker : Baker<RespawnableAuthoring>
        {
            public override void Bake(RespawnableAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Respawnable
                {
                    SpawnLocation = authoring.transform.position
                });
            }
        }
    }

    public struct Respawnable : IComponentData
    {
        public float3 SpawnLocation;
    }
}
