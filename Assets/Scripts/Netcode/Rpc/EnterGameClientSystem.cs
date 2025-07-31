using TwoOnPlane.Player;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace TwoOnPlane.Netcode.Rpc
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    partial struct EnterGameClientSystem : ISystem
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
            EntityCommandBuffer buffer = new(Allocator.Temp);
            foreach ((RefRO<NetworkId> networkId, Entity entity)
                in SystemAPI.Query<RefRO<NetworkId>>().WithEntityAccess().WithNone<NetworkStreamInGame>())
            {
                buffer.AddComponent<NetworkStreamInGame>(entity);
                Entity requestedPlayer = buffer.CreateEntity();
                buffer.AddComponent<EnterGameRpc>(requestedPlayer);
                buffer.AddComponent(requestedPlayer, new SendRpcCommandRequest
                {
                    TargetConnection = entity
                });
                //UnityEngine.Debug.Log($"Rpc sent from {networkId.ValueRO.Value}");
            }
            buffer.Playback(state.EntityManager);
        }
    }
}
