using NeonKolobok.Systems;

namespace NeonKolobok.Core
{
    public static class GameSession
    {
        public static GameMode SelectedMode = GameMode.Normal;
        public static float LastRunTime;
        public static int LastRunDeaths;

        public static void LoadLastMode()
        {
            SelectedMode = SaveLoadSystem.Load().lastMode;
        }
    }
}
