using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace TwoOnPlane.Coins
{
    [WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
    partial struct CoinSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<CoinSpawner>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            RefRW<CoinSpawner> spawner = SystemAPI.GetSingletonRW<CoinSpawner>();      
            if (spawner.ValueRO.Cooldown < 10)
            {
                spawner.ValueRW.Cooldown += SystemAPI.Time.DeltaTime;
                return;
            }    
            float range = spawner.ValueRO.SpawnRange;
            
            EntityCommandBuffer buffer = new EntityCommandBuffer(Allocator.Temp);
            spawner.ValueRW.Cooldown = 0;
            // Set spawn point
            float3 spawnOffset = new(UnityEngine.Random.Range(-range, range), 0, UnityEngine.Random.Range(-range, range));
            float3 spawnPosition = spawner.ValueRO.SpawnLocation + spawnOffset;
            // Create a coin
            Entity coin = buffer.Instantiate(spawner.ValueRO.Coin);
            buffer.SetComponent(coin, LocalTransform.FromPosition(spawnPosition));
            buffer.Playback(state.EntityManager);
        }
    }
}
