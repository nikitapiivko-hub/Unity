using UnityEngine;

namespace NeonKolobok.Systems
{
    public class AudioManager : MonoBehaviour
    {
        public enum SfxId { Jump, Death, Click }

        public static AudioManager Instance { get; private set; }

        private AudioSource _music;
        private AudioSource _sfx;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            _music = gameObject.AddComponent<AudioSource>();
            _sfx = gameObject.AddComponent<AudioSource>();
            _music.loop = true;
            _music.clip = BuildTone(110f, 3f, 0.15f);
            _music.Play();
            ApplyVolumes();
        }

        public void ApplyVolumes()
        {
            var data = SettingsManager.Instance != null ? SettingsManager.Instance.Data : SaveLoadSystem.Load();
            _music.volume = data.musicVolume;
            _sfx.volume = data.sfxVolume;
        }

        public void PlaySfx(SfxId id)
        {
            switch (id)
            {
                case SfxId.Jump:
                    _sfx.PlayOneShot(BuildTone(450f, 0.08f, 0.25f));
                    break;
                case SfxId.Death:
                    _sfx.PlayOneShot(BuildTone(140f, 0.2f, 0.3f));
                    break;
                case SfxId.Click:
                    _sfx.PlayOneShot(BuildTone(800f, 0.04f, 0.2f));
                    break;
            }
        }

        private AudioClip BuildTone(float frequency, float length, float amplitude)
        {
            var sampleRate = 44100;
            var samples = Mathf.CeilToInt(sampleRate * length);
            var data = new float[samples];
            for (var i = 0; i < samples; i++)
            {
                var env = Mathf.Exp(-4f * i / (float)samples);
                data[i] = Mathf.Sin(2 * Mathf.PI * frequency * i / sampleRate) * amplitude * env;
            }

            var clip = AudioClip.Create($"tone_{frequency}", samples, 1, sampleRate, false);
            clip.SetData(data, 0);
            return clip;
        }
    }
}
