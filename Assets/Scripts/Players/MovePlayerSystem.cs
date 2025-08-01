using System.Numerics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

namespace TwoOnPlane.Players
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    partial struct MovePlayerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRW<LocalTransform> localTransform, RefRO<CursorFollower> cursorFollower, RefRO<StatHolder> statHolder)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CursorFollower>, RefRO<StatHolder>>())
            {
                float3 targetPosition = new(cursorFollower.ValueRO.Horizontal, localTransform.ValueRO.Position.y, cursorFollower.ValueRO.Vertical);
                if (targetPosition.Equals(float3.zero)) continue;
                float3 directionUnit = math.normalize(targetPosition - localTransform.ValueRO.Position);
                float moveSpeed = statHolder.ValueRO.Speed + 3f;
                localTransform.ValueRW.Position += moveSpeed * deltaTime * directionUnit;
                float rotationY = math.Euler(localTransform.ValueRW.Rotation).y;
                localTransform.ValueRW.Rotation = quaternion.Euler(new float3(0, rotationY, 0));
            }
        }
    }
}
