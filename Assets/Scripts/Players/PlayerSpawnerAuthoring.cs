using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TwoOnPlane.Players
{
    public class PlayerSpawnerAuthoring : MonoBehaviour
    {
        public GameObject FirstPlayerPrefab;
        public GameObject SecondPlayerPrefab;
        public float SpawnRange;

        class Baker : Baker<PlayerSpawnerAuthoring>
        {
            public override void Bake(PlayerSpawnerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerSpawner
                    {
                        FirstPlayer = GetEntity(authoring.FirstPlayerPrefab, TransformUsageFlags.Dynamic),
                        SecondPlayer = GetEntity(authoring.SecondPlayerPrefab, TransformUsageFlags.Dynamic),
                        SpawnOrigin = authoring.transform.position + (Vector3.up * 1.2f), // spawning 1.2f above the plane
                        SpawnRange = authoring.SpawnRange
                    });
            }
        }
    }

    public struct PlayerSpawner : IComponentData
    {
        public Entity FirstPlayer;
        public Entity SecondPlayer;
        public float3 SpawnOrigin;
        public float SpawnRange;
    }
}
