using Sandbox;
using Sandbox.Game.World;
using Sandbox.ModAPI;
using System.IO;
using Torch;
using Torch.API;
using Torch.API.Managers;
using Torch.API.Session;
using Torch.Session;
using VRage.Game;
using VRage.Utils;

namespace LongSyncTorch
{
    public class LongSyncTorch : TorchPluginBase
    {
        public override void Init(ITorchBase torch)
        {
            base.Init(torch);

            var sessionManager = Torch.Managers.GetManager<TorchSessionManager>();
            if (sessionManager != null)
                sessionManager.SessionStateChanged += SessionChanged;
        }

        private void SessionChanged(ITorchSession session, TorchSessionState state)
        {
            switch (state)
            {
                case TorchSessionState.Loaded:
                    string loadWorld = MySandboxGame.ConfigDedicated.LoadWorld;
                    string finalPath = Path.Combine(loadWorld, "Sandbox_config.sbc");
                    var worldConfig = MyAPIGateway.Utilities.SerializeFromXML<MyObjectBuilder_WorldConfiguration>(File.ReadAllText(finalPath));
                    MyLog.Default.WriteLineAndConsole($"Pulling sync from settings: {worldConfig.Settings.SyncDistance}");
                    MyLog.Default.WriteLineAndConsole($"Pulling view distance from settings: {worldConfig.Settings.ViewDistance}");
                    MySession.Static.Settings.SyncDistance = worldConfig.Settings.SyncDistance;
                    MySession.Static.Settings.ViewDistance = worldConfig.Settings.ViewDistance;
                    break;

                case TorchSessionState.Unloading:
                    break;
            }
        }
    }
}
