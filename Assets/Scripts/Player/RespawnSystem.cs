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
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            PlayerSpawner spawner = SystemAPI.GetSingleton<PlayerSpawner>();
            float range = spawner.SpawnRange;
            foreach ((RefRW<LocalTransform> localTransform, RefRO<Respawnable> respawnable)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Respawnable>>())
            {
                if (localTransform.ValueRO.Position.y > -3) continue;
                float3 spawnOffset = new(UnityEngine.Random.Range(-range, range), 0, UnityEngine.Random.Range(-range, range));
                localTransform.ValueRW.Position = spawner.SpawnLocation + spawnOffset;
            }
        }
    }
}
