using TwoOnPlane.Players;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace TwoOnPlane.Hybrid
{
    [WorldSystemFilter(WorldSystemFilterFlags.ClientSimulation)]
    partial struct UpdateAnimationSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            foreach ((RefRO<CursorFollower> cursorFollower, SystemAPI.ManagedAPI.UnityEngineComponent<Animator> animator)
                in SystemAPI.Query<RefRO<CursorFollower>, SystemAPI.ManagedAPI.UnityEngineComponent<Animator>>())
            {
                animator.Value.SetBool("isMoving", cursorFollower.ValueRO.IsMoving);
            }
        }
    }
}
