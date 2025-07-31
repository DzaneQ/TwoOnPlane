using TwoOnPlane.Player;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace TwoOnPlane.Player
{
    public class PlayerSpawnerAuthoring : MonoBehaviour
    {
        public GameObject PlayerPrefab;
        public float SpawnRange;

        class Baker : Baker<PlayerSpawnerAuthoring>
        {
            public override void Bake(PlayerSpawnerAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new PlayerSpawner
                {
                    Player = GetEntity(authoring.PlayerPrefab, TransformUsageFlags.Dynamic),
                    SpawnLocation = authoring.transform.position + (Vector3.up * 1.2f), // spawning 1.2f above the plane
                    SpawnRange = authoring.SpawnRange
                });
            }
        }
    }

    public struct PlayerSpawner : IComponentData
    {
        public Entity Player;
        public float3 SpawnLocation;
        public float SpawnRange;
    }
}
