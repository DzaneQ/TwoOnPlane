using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace TwoOnPlane.Players
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    partial struct CursorInputSystem : ISystem
    {

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NetworkStreamInGame>();
            state.RequireForUpdate<CursorFollower>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            foreach (RefRW<CursorFollower> cursorFollower in SystemAPI.Query<RefRW<CursorFollower>>().WithAll<GhostOwnerIsLocal>())
            {
                if (!Input.GetMouseButtonDown(0)) continue;
                float3 inputPosition = Input.mousePosition;
                cursorFollower.ValueRW.Horizontal = inputPosition.x;
                cursorFollower.ValueRW.Vertical = inputPosition.y;
            }
        }
    }
}
