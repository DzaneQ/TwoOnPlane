using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Rendering;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

namespace TwoOnPlane.Hybrid
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    partial struct AddAnimatorSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach (Entity entity in SystemAPI.QueryBuilder().WithAll<HybridInstance>().WithNone<Animator>().Build().ToEntityArray(Allocator.Temp))
            {
                // Client should display a rendered game object
                HybridInstance hybridInstance = SystemAPI.ManagedAPI.GetComponent<HybridInstance>(entity);
                GameObject reference = Object.Instantiate(hybridInstance.PlayerReference);
                state.EntityManager.AddComponentObject(entity, reference.transform);
                state.EntityManager.AddComponentObject(entity, reference.GetComponent<Animator>());
                // Client shouldn't display the entity's render (which occurs in one of linked entities)
                NativeList<Entity> childEntities = new NativeList<Entity>(Allocator.Temp);
                DynamicBuffer<LinkedEntityGroup> buffer = state.EntityManager.GetBuffer<LinkedEntityGroup>(entity, true);
                foreach (LinkedEntityGroup linkedEntityGroup in buffer) childEntities.Add(linkedEntityGroup.Value);
                foreach (Entity linkedEntity in childEntities) state.EntityManager.AddComponentData(linkedEntity, new DisableRendering());
            }
        }
    }
}
