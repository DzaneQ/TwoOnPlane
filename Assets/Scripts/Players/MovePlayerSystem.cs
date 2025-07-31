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
    public partial class MovePlayerSystem : SystemBase
    {
        private Camera cam;

        protected override void OnStartRunning()
        {
            cam = Camera.main;
        }

        protected override void OnUpdate()
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRW<LocalTransform> localTransform, RefRO<CursorFollower> cursorFollower, RefRO<StatHolder> statHolder)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CursorFollower>, RefRO<StatHolder>>())
            {
                float3 inputPosition = new(cursorFollower.ValueRO.Horizontal, cursorFollower.ValueRO.Vertical, cursorFollower.ValueRO.CameraDistance);
                Debug.Log($"Input position: ({inputPosition.x}, {inputPosition.y}, {inputPosition.z})");
                float3 targetPosition = cam.ScreenToWorldPoint(inputPosition);
                Debug.Log($"Target position: ({targetPosition.x}, {targetPosition.y}, {targetPosition.z})");
                targetPosition.y = localTransform.ValueRO.Position.y;
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
