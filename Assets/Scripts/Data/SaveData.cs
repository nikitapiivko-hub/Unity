using System;
using NeonKolobok.Core;

namespace NeonKolobok.Data
{
    [Serializable]
    public class SaveData
    {
        public float bestTime = -1f;
        public int totalDeaths;
        public float musicVolume = 0.6f;
        public float sfxVolume = 0.8f;
        public bool fullscreen = false;
        public GameMode lastMode = GameMode.Normal;
        public int qualityIndex = 2;
    }
}
