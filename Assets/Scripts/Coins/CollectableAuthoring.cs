using TwoOnPlane.Players;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TwoOnPlane.Coins
{
    public class CollectableAuthoring : MonoBehaviour
    {
        class Baker : Baker<CollectableAuthoring>
        {
            public override void Bake(CollectableAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new Collectable
                {
                    RotationSpeed = 90f
                });
                AddComponent(entity, new StatHolder
                {
                    Speed = 1f
                });
            }
        }
    }

    public struct Collectable : IComponentData
    {
        public float RotationSpeed;
    }
}
