using TMPro;
using TwoOnPlane.Player;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TwoOnPlane.Player
{
    partial struct RespawnSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRW<LocalTransform> localTransform, RefRO<Respawnable> respawnable)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Respawnable>>())
            {
                if (localTransform.ValueRO.Position.y > -3) continue;
                localTransform.ValueRW.Position = respawnable.ValueRO.SpawnLocation;
            }
        }
    }
}
