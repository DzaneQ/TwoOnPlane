using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

namespace TwoOnPlane.Hybrid
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    partial struct HybridTransformSyncSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRO<LocalTransform> localTransform, SystemAPI.ManagedAPI.UnityEngineComponent<Transform> transform)
                in SystemAPI.Query<RefRO<LocalTransform>, SystemAPI.ManagedAPI.UnityEngineComponent<Transform>>())
            {
                transform.Value.position = localTransform.ValueRO.Position;
                transform.Value.rotation = localTransform.ValueRO.Rotation;
            }
        }
    }
}
