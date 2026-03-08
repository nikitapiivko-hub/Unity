using NeonKolobok.Core;
using UnityEngine;

namespace NeonKolobok.Environment
{
    public class LevelBuilder : MonoBehaviour
    {
        public void BuildLevel()
        {
            BuildBackground();
            BuildZone("Entry Rift", 0f, 25f, includeLasers: false, includeMachinery: false);
            BuildZone("Ember Shafts", 25f, 55f, includeLasers: false, includeMachinery: true);
            BuildZone("Laser Pits", 55f, 95f, includeLasers: true, includeMachinery: false);
            BuildZone("Infernal Machinery", 95f, 140f, includeLasers: true, includeMachinery: true);
            BuildFinalZone();
        }

        private void BuildBackground()
        {
            Camera.main.backgroundColor = new Color(0.04f, 0.01f, 0.08f);
            for (var i = 0; i < 30; i++)
            {
                var rune = GameObject.CreatePrimitive(PrimitiveType.Quad);
                Destroy(rune.GetComponent<Collider>());
                rune.name = $"Rune_{i}";
                rune.transform.position = new Vector3(Random.Range(-9f, 9f), Random.Range(0f, 170f), 5f);
                rune.transform.localScale = Vector3.one * Random.Range(0.2f, 0.8f);
                var renderer = rune.GetComponent<MeshRenderer>();
                renderer.material = new Material(Shader.Find("Sprites/Default"));
                renderer.material.color = new Color(1f, 0.2f, 0.4f, 0.08f);
            }
        }

        private void BuildZone(string zoneName, float yMin, float yMax, bool includeLasers, bool includeMachinery)
        {
            var parent = new GameObject(zoneName).transform;
            for (var y = yMin; y < yMax; y += 5f)
            {
                var platformX = Random.Range(-4.5f, 4.5f);
                var platform = CreatePlatform(new Vector2(platformX, y), new Vector2(2.6f, 0.4f), parent);

                if (Random.value > 0.55f)
                {
                    platform.AddComponent<MovingPlatform>().enabled = y > 20f;
                }

                if (y > 80f && Random.value > 0.65f)
                {
                    platform.AddComponent<DisappearingPlatform>();
                }

                if (Random.value > 0.72f)
                {
                    CreateHazard(new Vector2(platformX + 1.9f, y + 0.35f), new Vector2(0.6f, 0.6f), parent, includeLasers && Random.value > 0.5f);
                }

                if (includeMachinery && Random.value > 0.75f)
                {
                    var saw = CreateHazard(new Vector2(platformX - 2.1f, y + 1f), new Vector2(0.8f, 0.8f), parent, false);
                    saw.AddComponent<RotatingHazard>();
                }

                if (GameSession.SelectedMode == GameMode.Practice && Mathf.Abs(y % 20f) < 0.2f)
                {
                    CreateCheckpoint(new Vector2(0f, y + 1.5f), parent);
                }
            }

            if (includeLasers)
            {
                for (var i = 0; i < 8; i++)
                {
                    var ly = Random.Range(yMin + 4f, yMax - 2f);
                    CreateLaser(new Vector2(0f, ly), new Vector2(9f, 0.25f), parent);
                }
            }
        }

        private void BuildFinalZone()
        {
            var parent = new GameObject("Crown of Hell").transform;
            for (var i = 0; i < 14; i++)
            {
                var y = 145f + i * 3.5f;
                var x = i % 2 == 0 ? -3.2f : 3.2f;
                CreatePlatform(new Vector2(x, y), new Vector2(2.2f, 0.35f), parent);
                CreateHazard(new Vector2(0f, y + 0.2f), new Vector2(1.2f, 0.35f), parent, true);
            }

            var finish = new GameObject("Finish", typeof(BoxCollider2D), typeof(FinishZone), typeof(SpriteRenderer));
            finish.transform.position = new Vector3(0f, 196f, 0f);
            finish.transform.localScale = new Vector3(4f, 1f, 1f);
            finish.GetComponent<BoxCollider2D>().isTrigger = true;
            finish.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.9f, 0.2f, 0.8f);
        }

        private GameObject CreatePlatform(Vector2 pos, Vector2 size, Transform parent)
        {
            var obj = new GameObject("Platform", typeof(BoxCollider2D), typeof(SpriteRenderer));
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            obj.transform.localScale = new Vector3(size.x, size.y, 1f);
            var col = obj.GetComponent<BoxCollider2D>();
            col.sharedMaterial = null;
            obj.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.08f, 0.25f, 1f);
            return obj;
        }

        private GameObject CreateHazard(Vector2 pos, Vector2 size, Transform parent, bool laser)
        {
            var obj = new GameObject(laser ? "LaserHazard" : "SpikeHazard", typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Hazard));
            obj.transform.SetParent(parent);
            obj.transform.position = pos;
            obj.transform.localScale = new Vector3(size.x, size.y, 1f);
            obj.GetComponent<BoxCollider2D>().isTrigger = true;
            obj.GetComponent<SpriteRenderer>().color = laser ? new Color(1f, 0.12f, 0.3f, 0.95f) : new Color(1f, 0.35f, 0.1f, 0.95f);
            if (laser) obj.AddComponent<LaserHazard>();
            return obj;
        }

        private void CreateLaser(Vector2 pos, Vector2 size, Transform parent)
        {
            CreateHazard(pos, size, parent, true);
        }

        private void CreateCheckpoint(Vector2 pos, Transform parent)
        {
            var cp = new GameObject("Checkpoint", typeof(BoxCollider2D), typeof(SpriteRenderer), typeof(Checkpoint));
            cp.transform.SetParent(parent);
            cp.transform.position = pos;
            cp.transform.localScale = new Vector3(1f, 1f, 1f);
            var col = cp.GetComponent<BoxCollider2D>();
            col.isTrigger = true;
            cp.GetComponent<SpriteRenderer>().color = new Color(0.2f, 0.6f, 1f, 0.7f);
        }
    }
}
