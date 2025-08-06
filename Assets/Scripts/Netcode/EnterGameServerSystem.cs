using System.Linq;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace TwoOnPlane.Netcode
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    partial struct EnterGameServerSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<PlayerSpawner>();
            state.RequireForUpdate<FirstPlayer>();
            state.RequireForUpdate<SecondPlayer>();
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
                int playerCount = 0;
                foreach (RefRO<GhostOwner> _ in SystemAPI.Query<RefRO<GhostOwner>>())
                {
                    playerCount++;
                }    
                if (playerCount >= 2)
                {
                    buffer.DestroyEntity(request);
                    continue;
                }    
                buffer.AddComponent<NetworkStreamInGame>(requestSrc.ValueRO.SourceConnection);
                int sourceId = SystemAPI.GetComponent<NetworkId>(requestSrc.ValueRO.SourceConnection).Value;
                Entity player = Entity.Null;
                // If first player exists, instantiate second player
                foreach (RefRO<FirstPlayer> _ in SystemAPI.Query<RefRO<FirstPlayer>>())
                {
                    player = buffer.Instantiate(spawner.SecondPlayer);
                    buffer.AddComponent(player, new SecondPlayer());
                    break;
                }
                // If second player not instantiated, instantiate first player
                if (player == Entity.Null)
                {
                    player = buffer.Instantiate(spawner.FirstPlayer);
                    buffer.AddComponent(player, new FirstPlayer());
                }
                buffer.SetComponent(player, LocalTransform.FromPosition(new float3(0, -5, 0)));
                buffer.AddComponent(player, new GhostOwner
                {
                    NetworkId = sourceId
                });
                buffer.AppendToBuffer(requestSrc.ValueRO.SourceConnection, new LinkedEntityGroup { 
                    Value = player
                });
                buffer.DestroyEntity(request);
            }
            buffer.Playback(state.EntityManager);
            buffer.Dispose();
        }
    }
}
