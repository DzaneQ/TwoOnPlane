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

namespace TwoOnPlane.Players
{
    public partial class AddAnimatorSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach ((RefRO<FirstPlayer> firstPlayer, Entity entity) in SystemAPI.Query<RefRO<FirstPlayer>>().WithEntityAccess().WithNone<Animator>())
            {
                //EntityManager.AddComponentObject(entity, AnimatorReadSingleton.s_Instance.FirstAnimator);
            }
            foreach ((RefRO<SecondPlayer> firstPlayer, Entity entity) in SystemAPI.Query<RefRO<SecondPlayer>>().WithEntityAccess().WithNone<Animator>())
            {
                //EntityManager.AddComponentObject(entity, AnimatorReadSingleton.s_Instance.SecondAnimator);
            }
        }
    }
}
