using Unity.Entities;
using UnityEngine;

namespace TwoOnPlane.Players
{
    public class StatHolderAuthoring : MonoBehaviour
    {
        public float Speed;

        class Baker : Baker<StatHolderAuthoring>
        {
            public override void Bake(StatHolderAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new StatHolder
                {
                    Speed = authoring.Speed,
                });
            }
        }
    }

    public struct StatHolder : IComponentData
    {
        public float Speed;
    }
}
