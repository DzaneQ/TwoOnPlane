using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TwoOnPlane.Players
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
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
            foreach ((RefRW<LocalTransform> localTransform, RefRO<Respawnable> respawnable, RefRW<CursorFollower> cursorFollower)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Respawnable>, RefRW<CursorFollower>>())
            {
                // Don't respawn when character is not below the plane
                if (localTransform.ValueRO.Position.y > -3) continue;
                // Set spawn point
                float3 spawnOffset = new(UnityEngine.Random.Range(-range, range), 0, UnityEngine.Random.Range(-range, range));
                localTransform.ValueRW.Position = spawner.SpawnLocation + spawnOffset;
                // Set cursor target to the spawn point
                cursorFollower.ValueRW.Horizontal = localTransform.ValueRO.Position.x;
                cursorFollower.ValueRW.Vertical = localTransform.ValueRO.Position.z;
                //UnityEngine.Debug.Log($"Respawn on local transform: {localTransform.ValueRO.Position.x}, {localTransform.ValueRO.Position.z}");
                //UnityEngine.Debug.Log($"Cursor following to: {cursorFollower.ValueRO.Horizontal}, {cursorFollower.ValueRO.Vertical}");
            }
        }
    }
}
