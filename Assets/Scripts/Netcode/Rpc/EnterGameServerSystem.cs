using System.Linq;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace TwoOnPlane.Netcode.Rpc
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    partial struct EnterGameServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerSpawner>();
            state.RequireForUpdate<NetworkId>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer buffer = new EntityCommandBuffer(Allocator.Temp);
            PlayerSpawner spawner = SystemAPI.GetSingleton<PlayerSpawner>();
            float range = spawner.SpawnRange;

            foreach ((RefRO<ReceiveRpcCommandRequest> requestSrc, Entity request) 
                in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithEntityAccess())
            {
                int playerCount = state.GetEntityQuery(ComponentType.ReadOnly<GhostOwner>()).CalculateEntityCount();
                if (playerCount >= 2)
                {
                    buffer.DestroyEntity(request);
                    continue;
                }    
                buffer.AddComponent<NetworkStreamInGame>(requestSrc.ValueRO.SourceConnection);
                int sourceId = SystemAPI.GetComponent<NetworkId>(requestSrc.ValueRO.SourceConnection).Value;
                Entity player = buffer.Instantiate(spawner.Player);
                float3 spawnOffset = new(UnityEngine.Random.Range(-range, range), 0, UnityEngine.Random.Range(-range, range));
                float3 spawnLocation = spawner.SpawnLocation + spawnOffset;
                buffer.SetComponent(player, LocalTransform.FromPosition(spawnLocation));
                buffer.AddComponent(player, new GhostOwner
                {
                    NetworkId = sourceId
                });
                //UnityEngine.Debug.Log($"Rpc received from: {sourceId}");
                buffer.DestroyEntity(request);
            }
            buffer.Playback(state.EntityManager);
        }
    }
}
