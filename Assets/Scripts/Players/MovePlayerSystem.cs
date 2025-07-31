using System.Numerics;
using TwoOnPlane.Singleton;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using Unity.VisualScripting;

namespace TwoOnPlane.Players
{
    public partial class MovePlayerSystem : SystemBase
    {
        private CursorSingleton _cursorSingleton;

        protected override void OnStartRunning()
        {
            _cursorSingleton = CursorSingleton.s_Instance;
        }

        protected override void OnUpdate()
        {
            float3 targetPosition = _cursorSingleton.GetCursorWorldPosition();
            float deltaTime = SystemAPI.Time.DeltaTime;

            foreach ((RefRW<LocalTransform> localTransform, RefRO<CursorFollower> cursorFollower, RefRO<StatHolder> statHolder)
                in SystemAPI.Query<RefRW<LocalTransform>, RefRO<CursorFollower>, RefRO<StatHolder>>())
            {
                targetPosition.y = localTransform.ValueRO.Position.y;
                if (targetPosition.Equals(float3.zero)) continue;
                float3 directionUnit = math.normalize(targetPosition - localTransform.ValueRO.Position);
                float moveSpeed = statHolder.ValueRO.Speed + 3f;
                localTransform.ValueRW.Position += moveSpeed * deltaTime * directionUnit;
                float rotationY = math.Euler(localTransform.ValueRW.Rotation).y;
                localTransform.ValueRW.Rotation = quaternion.Euler(new float3(0, rotationY, 0));
            }
        }
    }
}
