using NeonKolobok.Core;
using NeonKolobok.Systems;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NeonKolobok.UI
{
    public class UIManager : MonoBehaviour
    {
        private Text _hud;
        private GameObject _pausePanel;
        private TimerSystem _timer;
        private RespawnSystem _respawn;

        public void InitGameplayUi(TimerSystem timer, RespawnSystem respawn)
        {
            _timer = timer;
            _respawn = respawn;
            CreateHud();
            CreatePause();
        }

        private void CreateHud()
        {
            var canvas = BuildCanvas("HUD");
            _hud = BuildText(canvas.transform, "HUDText", new Vector2(20, -20), TextAnchor.UpperLeft, 20);
        }

        private void CreatePause()
        {
            var canvas = BuildCanvas("Pause");
            _pausePanel = new GameObject("PausePanel", typeof(Image));
            _pausePanel.transform.SetParent(canvas.transform, false);
            var rect = _pausePanel.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.25f, 0.25f);
            rect.anchorMax = new Vector2(0.75f, 0.75f);
            rect.offsetMin = rect.offsetMax = Vector2.zero;
            _pausePanel.GetComponent<Image>().color = new Color(0f, 0f, 0f, 0.7f);
            BuildText(_pausePanel.transform, "PauseLabel", Vector2.zero, TextAnchor.MiddleCenter, 32, "PAUSED");
            _pausePanel.SetActive(false);
        }

        private void Update()
        {
            if (_hud != null && _timer != null && _respawn != null)
            {
                _hud.text = $"MODE: {GameSession.SelectedMode}\nTIME: {_timer.Elapsed:0.00}\nDEATHS: {_respawn.Deaths}";
            }

            if (Input.GetKeyDown(KeyCode.Escape) && _pausePanel != null)
            {
                var paused = !_pausePanel.activeSelf;
                _pausePanel.SetActive(paused);
                Time.timeScale = paused ? 0f : 1f;
            }

            if (Input.GetKeyDown(KeyCode.F11) && SettingsManager.Instance != null)
            {
                SettingsManager.Instance.ToggleFullscreen();
            }
        }

        public void BuildMainMenu()
        {
            var canvas = BuildCanvas("MainMenuCanvas");
            BuildText(canvas.transform, "Title", new Vector2(0, 250), TextAnchor.MiddleCenter, 48, "NEON KOLOBOK\nHELL TOWER");
            BuildButton(canvas.transform, "Start", new Vector2(0, 80), "START", () => SceneManager.LoadScene("Game"));
            BuildButton(canvas.transform, "Mode", new Vector2(0, 20), $"MODE: {GameSession.SelectedMode}", () =>
            {
                GameSession.SelectedMode = (GameMode)(((int)GameSession.SelectedMode + 1) % 3);
                SaveMode();
                SceneManager.LoadScene("MainMenu");
            });
            BuildButton(canvas.transform, "Settings", new Vector2(0, -40), "TOGGLE FULLSCREEN", () => SettingsManager.Instance?.ToggleFullscreen());
            BuildButton(canvas.transform, "Quit", new Vector2(0, -100), "QUIT", Application.Quit);
        }

        public void BuildResults()
        {
            var canvas = BuildCanvas("ResultsCanvas");
            BuildText(canvas.transform, "Result", new Vector2(0, 120), TextAnchor.MiddleCenter, 36,
                $"YOU SURVIVED\nTIME: {GameSession.LastRunTime:0.00}\nDEATHS: {GameSession.LastRunDeaths}");

            var data = SaveLoadSystem.Load();
            if (data.bestTime < 0 || GameSession.LastRunTime < data.bestTime)
            {
                data.bestTime = GameSession.LastRunTime;
            }

            data.totalDeaths += GameSession.LastRunDeaths;
            SaveLoadSystem.Save(data);

            BuildText(canvas.transform, "Best", new Vector2(0, 20), TextAnchor.MiddleCenter, 24,
                $"BEST TIME: {data.bestTime:0.00}\nTOTAL DEATHS: {data.totalDeaths}");
            BuildButton(canvas.transform, "Retry", new Vector2(0, -80), "TRY AGAIN", () => SceneManager.LoadScene("Game"));
            BuildButton(canvas.transform, "Menu", new Vector2(0, -140), "MAIN MENU", () => SceneManager.LoadScene("MainMenu"));
        }

        private static Canvas BuildCanvas(string name)
        {
            var canvasObj = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
            var canvas = canvasObj.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObj.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1280, 720);
            return canvas;
        }

        private static Text BuildText(Transform parent, string name, Vector2 anchoredPos, TextAnchor anchor, int size, string content = "")
        {
            var go = new GameObject(name, typeof(Text));
            go.transform.SetParent(parent, false);
            var text = go.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.text = content;
            text.alignment = anchor;
            text.fontSize = size;
            text.color = new Color(1f, 0.35f, 0.85f);
            var rect = text.rectTransform;
            rect.anchorMin = rect.anchorMax = anchor == TextAnchor.UpperLeft ? new Vector2(0, 1) : new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPos;
            rect.sizeDelta = new Vector2(700, 220);
            return text;
        }

        private static void BuildButton(Transform parent, string name, Vector2 pos, string label, UnityEngine.Events.UnityAction action)
        {
            var btnObj = new GameObject(name, typeof(Image), typeof(Button));
            btnObj.transform.SetParent(parent, false);
            var rect = btnObj.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(320, 48);
            rect.anchoredPosition = pos;
            btnObj.GetComponent<Image>().color = new Color(0.1f, 0f, 0.2f, 0.85f);
            var button = btnObj.GetComponent<Button>();
            button.onClick.AddListener(() => AudioManager.Instance?.PlaySfx(AudioManager.SfxId.Click));
            button.onClick.AddListener(action);

            var text = BuildText(btnObj.transform, "Label", Vector2.zero, TextAnchor.MiddleCenter, 20, label);
            text.rectTransform.sizeDelta = rect.sizeDelta;
        }

        private static void SaveMode()
        {
            var data = SaveLoadSystem.Load();
            data.lastMode = GameSession.SelectedMode;
            SaveLoadSystem.Save(data);
        }
    }
}
