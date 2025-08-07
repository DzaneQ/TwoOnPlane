using System.Numerics;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace TwoOnPlane.Players
{
    [UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
    partial struct MovePlayerSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRW<LocalTransform> localTransform, RefRW<CursorFollower> cursorFollower, RefRO<StatHolder> statHolder)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRW<CursorFollower>, RefRO<StatHolder>>())
            {
                float3 targetPosition = new(cursorFollower.ValueRO.Horizontal, localTransform.ValueRO.Position.y, cursorFollower.ValueRO.Vertical);
                float3 path = targetPosition - localTransform.ValueRO.Position;
                // If player's location is almost at the the cursor, don't move
                if (math.length(path) < 0.01f)
                {
                    float rotationY = math.Euler(localTransform.ValueRW.Rotation).y;
                    localTransform.ValueRW.Rotation = quaternion.Euler(new float3(0, rotationY, 0));
                    cursorFollower.ValueRW.IsMoving = false;
                    continue;
                }
                cursorFollower.ValueRW.IsMoving = true;
                float3 directionUnit = math.normalize(path);
                float moveSpeed = statHolder.ValueRO.Speed + 3f;
                float3 distance = moveSpeed * deltaTime * directionUnit;
                // If player has to move less than the frame distance, set position to the cursor 
                if (math.length(distance) >= math.length(path))
                {
                    localTransform.ValueRW.Position = targetPosition;
                    continue;
                }
                localTransform.ValueRW.Position += distance;
                // Rotate player towards where player moves
                float newRotationY = math.Euler(quaternion.LookRotation(directionUnit, math.up())).y;
                localTransform.ValueRW.Rotation = quaternion.Euler(new float3(0, newRotationY, 0));
            }
        }
    }
}
