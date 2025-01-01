using MelonLoader;

namespace SilentWalker
{
    public class Main : MelonMod
    {
        internal static MelonLogger.Instance Logger { get; } = new MelonLogger.Instance(BuildInfo.Name);
        public override void OnInitializeMelon()
        {
            Logger.Msg($"Version {Info.Version} loaded!");
            Settings.OnLoad();
        }
    }
}

