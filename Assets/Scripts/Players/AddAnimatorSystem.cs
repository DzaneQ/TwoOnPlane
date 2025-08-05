using TMPro;
using TwoOnPlane.Players;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using Unity.VisualScripting;

namespace TwoOnPlane.Players
{
    public partial class AddAnimatorSystem : SystemBase
    {
        protected override void OnStartRunning()
        {

        }

        protected override void OnUpdate()
        {
            foreach ((RefRO<GhostOwner> ghostOwner, Entity entity) in SystemAPI.Query<RefRO<GhostOwner>>().WithEntityAccess())
            {
                
            }
        }
    }
}
