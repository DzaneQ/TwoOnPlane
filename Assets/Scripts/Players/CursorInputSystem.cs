using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

namespace TwoOnPlane.Players
{
    [UpdateInGroup(typeof(GhostInputSystemGroup))]
    public partial class CursorInputSystem : SystemBase
    {
        private Camera _cam;
        private float _distance;

        protected override void OnStartRunning()
        {
            RequireForUpdate<NetworkStreamInGame>();
            RequireForUpdate<CursorFollower>();
        }

        protected override void OnUpdate()
        {
            if (_cam == null)
            {
                _cam = Camera.main;
                if (_cam == null) return;
                _distance = _cam.transform.position.y;
            }
            foreach (RefRW<CursorFollower> cursorFollower in SystemAPI.Query<RefRW<CursorFollower>>().WithAll<GhostOwnerIsLocal>())
            {
                if (!Input.GetMouseButtonDown(0)) continue;
                float3 inputPosition = Input.mousePosition;
                inputPosition.z = _distance;
                float3 worldPosition = _cam.ScreenToWorldPoint(inputPosition);
                cursorFollower.ValueRW.Horizontal = worldPosition.x;
                cursorFollower.ValueRW.Vertical = worldPosition.z;
            }
        }
    }
}
