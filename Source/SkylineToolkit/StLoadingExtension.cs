using ColossalFramework;
using ICities;

namespace SkylineToolkit
{
    public sealed class StLoadingExtension : LoadingExtensionBase
    {
        public override void OnLevelLoaded(LoadMode mode)
        {
            ReactivateAchivements();
        }

        private static void ReactivateAchivements()
        {
            Singleton<SimulationManager>.instance.m_metaData.m_disableAchievements = SimulationMetaData.MetaBool.False;

            Log.Info("Re-enabled achivements.");
        }
    }
}
