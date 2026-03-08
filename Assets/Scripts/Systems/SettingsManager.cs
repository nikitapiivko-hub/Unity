using NeonKolobok.Data;
using UnityEngine;

namespace NeonKolobok.Systems
{
    public class SettingsManager : MonoBehaviour
    {
        public static SettingsManager Instance { get; private set; }
        public SaveData Data { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Data = SaveLoadSystem.Load();
            ApplySettings();
        }

        public void SetMusic(float value)
        {
            Data.musicVolume = value;
            Save();
            AudioManager.Instance?.ApplyVolumes();
        }

        public void SetSfx(float value)
        {
            Data.sfxVolume = value;
            Save();
            AudioManager.Instance?.ApplyVolumes();
        }

        public void ToggleFullscreen()
        {
            Data.fullscreen = !Data.fullscreen;
            ApplySettings();
            Save();
        }

        public void ApplySettings()
        {
            Screen.fullScreen = Data.fullscreen;
            Screen.SetResolution(1280, 720, Data.fullscreen);
            QualitySettings.SetQualityLevel(Data.qualityIndex, true);
        }

        public void Save()
        {
            SaveLoadSystem.Save(Data);
        }
    }
}
