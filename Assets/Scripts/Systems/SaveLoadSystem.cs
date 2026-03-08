using System.IO;
using NeonKolobok.Data;
using UnityEngine;

namespace NeonKolobok.Systems
{
    public static class SaveLoadSystem
    {
        private const string FileName = "neon_kolobok_save.json";

        private static string SavePath => Path.Combine(Application.persistentDataPath, FileName);

        public static SaveData Load()
        {
            if (!File.Exists(SavePath))
            {
                return new SaveData();
            }

            var json = File.ReadAllText(SavePath);
            return JsonUtility.FromJson<SaveData>(json) ?? new SaveData();
        }

        public static void Save(SaveData data)
        {
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(SavePath, json);
        }
    }
}
