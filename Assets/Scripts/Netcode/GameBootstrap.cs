using Unity.NetCode;

namespace TwoOnPlane.Netcode
{
    [UnityEngine.Scripting.Preserve]
    public class GameBootstrap : ClientServerBootstrap
    {
        public override bool Initialize(string defaultWorldName)
        {
            AutoConnectPort = 7979; // Enabled auto connect
            return base.Initialize(defaultWorldName); // Use the regular bootstrap
        }
    }
}