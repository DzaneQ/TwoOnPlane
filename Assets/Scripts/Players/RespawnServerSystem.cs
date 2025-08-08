using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace TwoOnPlane.Players
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    partial struct RespawnServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer buffer = new(Allocator.Temp);
            PlayerSpawner spawner = SystemAPI.GetSingleton<PlayerSpawner>();
            float range = spawner.SpawnRange;
            foreach ((RefRW<LocalTransform> localTransform, RefRW<Respawnable> respawnable, RefRW<CursorFollower> cursorFollower, Entity entity)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Respawnable>, RefRW<CursorFollower>>().WithEntityAccess())
            {
                // Don't respawn when character is not below the plane
                if (localTransform.ValueRO.Position.y > -3) continue;
                // Set spawn point
                float3 spawnOffset = new(UnityEngine.Random.Range(-range, range), 0, UnityEngine.Random.Range(-range, range));
                float3 spawnLocation = spawner.SpawnOrigin + spawnOffset;
                localTransform.ValueRW.Position = spawnLocation;
                // Set cursor target to the spawn point and update data on client
                cursorFollower.ValueRW.Horizontal = spawnLocation.x;
                cursorFollower.ValueRW.Vertical = spawnLocation.z;
                cursorFollower.ValueRW.IsMoving = false;
                Entity request = buffer.CreateEntity();
                buffer.AddComponent(request, new UpdateCursorRpc
                {
                    Horizontal = spawnLocation.x,
                    Vertical = spawnLocation.z,
                    Player = entity
                });
                buffer.AddComponent(request, new SendRpcCommandRequest());
            }
            buffer.Playback(state.EntityManager);
        }

        public struct UpdateCursorRpc : IRpcCommand
        {
            public float Horizontal;
            public float Vertical;
            public Entity Player;
        }
    }
}
