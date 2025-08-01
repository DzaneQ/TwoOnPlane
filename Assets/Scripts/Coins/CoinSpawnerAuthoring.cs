using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TwoOnPlane.Coins
{
    public class CoinSpawnerAuthoring : MonoBehaviour
    {
        public GameObject CoinPrefab;

        class Baker : Baker<CoinSpawnerAuthoring>
        {
            public override void Bake(CoinSpawnerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);


                AddComponent(entity, new CoinSpawner
                {
                    Coin = GetEntity(authoring.CoinPrefab, TransformUsageFlags.Dynamic),
                    SpawnLocation = authoring.transform.position + (Vector3.up * 0.4f), // spawning 0.4f above the plane
                    SpawnRange = authoring.transform.localScale.x * 4.5f
                });
            }
        }
    }

    public struct CoinSpawner : IComponentData
    {
        public Entity Coin;
        public float3 SpawnLocation;
        public float SpawnRange;
        public float Cooldown;
    }
}
