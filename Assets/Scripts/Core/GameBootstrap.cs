using NeonKolobok.Environment;
using NeonKolobok.Player;
using NeonKolobok.Systems;
using NeonKolobok.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace NeonKolobok.Core
{
    public static class GameBootstrap
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void EnsurePersistentSystems()
        {
            if (Object.FindObjectOfType<SettingsManager>() == null)
            {
                var settings = new GameObject("SettingsManager", typeof(SettingsManager));
                Object.DontDestroyOnLoad(settings);
            }

            if (Object.FindObjectOfType<AudioManager>() == null)
            {
                var audio = new GameObject("AudioManager", typeof(AudioManager));
                Object.DontDestroyOnLoad(audio);
            }

            if (Object.FindObjectOfType<EventSystem>() == null)
            {
                new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            }

            GameSession.LoadLastMode();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Time.timeScale = 1f;
            if (Camera.main == null)
            {
                var camObj = new GameObject("Main Camera", typeof(Camera), typeof(AudioListener), typeof(CameraEffects));
                camObj.tag = "MainCamera";
                camObj.GetComponent<Camera>().orthographic = true;
                camObj.GetComponent<Camera>().orthographicSize = 6.2f;
            }

            var ui = new GameObject("UIManager", typeof(UIManager)).GetComponent<UIManager>();

            switch (scene.name)
            {
                case "Boot":
                    SceneManager.LoadScene("MainMenu");
                    break;
                case "MainMenu":
                    ui.BuildMainMenu();
                    break;
                case "Game":
                    BuildGameplay(ui);
                    break;
                case "Results":
                    ui.BuildResults();
                    break;
            }
        }

        private static void BuildGameplay(UIManager ui)
        {
            var groundMask = LayerMask.GetMask("Default");
            var player = BuildPlayer(groundMask);
            var respawn = new GameObject("RespawnSystem", typeof(RespawnSystem)).GetComponent<RespawnSystem>();
            var timer = new GameObject("TimerSystem", typeof(TimerSystem)).GetComponent<TimerSystem>();
            respawn.Initialize(player, GameSession.SelectedMode == GameMode.Hardcore);
            player.RespawnSystem = respawn;

            var level = new GameObject("LevelBuilder", typeof(LevelBuilder)).GetComponent<LevelBuilder>();
            level.BuildLevel();

            var lava = new GameObject("Lava", typeof(BoxCollider2D), typeof(Hazard), typeof(SpriteRenderer));
            lava.transform.position = new Vector3(0f, -2f, 0f);
            lava.transform.localScale = new Vector3(12f, 2f, 1f);
            lava.GetComponent<BoxCollider2D>().isTrigger = true;
            lava.GetComponent<SpriteRenderer>().color = new Color(1f, 0.2f, 0.05f, 0.75f);

            ui.InitGameplayUi(timer, respawn);
        }

        private static PlayerController BuildPlayer(LayerMask groundMask)
        {
            var playerObj = new GameObject("NeonKolobok", typeof(Rigidbody2D), typeof(CircleCollider2D), typeof(SpriteRenderer), typeof(TrailRenderer), typeof(PlayerController));
            playerObj.transform.position = new Vector3(0f, 1.5f, 0f);
            playerObj.transform.localScale = Vector3.one * 0.8f;

            var rb = playerObj.GetComponent<Rigidbody2D>();
            rb.gravityScale = 3.4f;
            rb.freezeRotation = true;

            var sr = playerObj.GetComponent<SpriteRenderer>();
            sr.sprite = CreateCircleSprite();
            sr.color = new Color(1f, 0.75f, 0.2f, 1f);

            var trail = playerObj.GetComponent<TrailRenderer>();
            trail.time = 0.2f;
            trail.startWidth = 0.22f;
            trail.endWidth = 0f;
            trail.material = new Material(Shader.Find("Sprites/Default"));
            trail.startColor = new Color(1f, 0.4f, 0.95f, 0.8f);
            trail.endColor = new Color(1f, 0.4f, 0.95f, 0f);

            var groundCheck = new GameObject("GroundCheck").transform;
            groundCheck.SetParent(playerObj.transform);
            groundCheck.localPosition = new Vector3(0f, -0.45f, 0f);

            var pc = playerObj.GetComponent<PlayerController>();
            var cam = Camera.main.GetComponent<CameraFollow>() ?? Camera.main.gameObject.AddComponent<CameraFollow>();
            cam.SetTarget(playerObj.transform);

            pc.Configure(groundCheck, groundMask);

            return pc;
        }

        private static Sprite CreateCircleSprite()
        {
            const int size = 64;
            var tex = new Texture2D(size, size);
            var center = new Vector2(size / 2f, size / 2f);
            var radius = size * 0.45f;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var d = Vector2.Distance(new Vector2(x, y), center);
                    var glow = Mathf.Clamp01(1f - d / radius);
                    var col = d < radius ? new Color(1f, 0.76f, 0.2f, 1f) : new Color(1f, 0.2f, 0.8f, glow * 0.2f);
                    tex.SetPixel(x, y, col);
                }
            }

            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), Vector2.one * 0.5f, size);
        }
    }
}
