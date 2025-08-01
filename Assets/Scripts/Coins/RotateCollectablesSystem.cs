using System.Diagnostics;
using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

namespace TwoOnPlane.Coins
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    partial struct RotateCollectablesSystem : ISystem
    {
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRW<LocalTransform> localTransform, RefRO<Collectable> collectable)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Collectable>>())
            {
                float3 currentRotation = math.Euler(localTransform.ValueRO.Rotation);
                currentRotation.y += deltaTime * collectable.ValueRO.RotationSpeed;
                localTransform.ValueRW.Rotation = quaternion.Euler(currentRotation);
            }
        }
    }
}
