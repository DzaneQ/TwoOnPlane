using TMPro;
using TwoOnPlane.Netcode;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using static TwoOnPlane.Players.RespawnServerSystem;

namespace TwoOnPlane.Players
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    partial struct RespawnClientSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            EntityCommandBuffer buffer = new EntityCommandBuffer(Allocator.Temp);
            foreach ((RefRO<ReceiveRpcCommandRequest> requestSrc, RefRO<UpdateCursorRpc> rpc, Entity request)
                in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>, RefRO<UpdateCursorRpc>>().WithEntityAccess())
            {
                foreach ((RefRW<CursorFollower> cursorFollower, Entity entity)
                    in SystemAPI.Query<RefRW<CursorFollower>>().WithEntityAccess())
                {
                    if (rpc.ValueRO.Player != entity) continue;
                    cursorFollower.ValueRW.Horizontal = rpc.ValueRO.Horizontal;
                    cursorFollower.ValueRW.Vertical = rpc.ValueRO.Vertical;
                    cursorFollower.ValueRW.IsMoving = false;
                }
                buffer.DestroyEntity(request);
            }
            buffer.Playback(state.EntityManager);
        }
    }
}
