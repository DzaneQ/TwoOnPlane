using Unity.Entities;
using Unity.NetCode;
using UnityEngine;

namespace TwoOnPlane.Hybrid
{
    public class HybridInstanceAuthoring : MonoBehaviour
    {
        class Baker : Baker<HybridInstanceAuthoring>
        {
            public override void Bake(HybridInstanceAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.WorldSpace);

                AddComponentObject(entity, new HybridInstance
                {
                    PlayerReference = authoring.gameObject
                });
            }
        }   
    }

    public class HybridInstance : IComponentData
    {
        public GameObject PlayerReference;
    }
}
