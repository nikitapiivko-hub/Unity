using UnityEngine;

namespace NeonKolobok.Environment
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class LaserHazard : MonoBehaviour
    {
        [SerializeField] private float activeDuration = 1.2f;
        [SerializeField] private float cooldown = 0.8f;

        private Collider2D _col;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _col = GetComponent<Collider2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var cycle = activeDuration + cooldown;
            var active = (Time.time % cycle) < activeDuration;
            _col.enabled = active;
            _renderer.color = active ? new Color(1f, 0.15f, 0.3f, 0.95f) : new Color(1f, 0.15f, 0.3f, 0.3f);
            transform.localScale = new Vector3(1f + Mathf.Sin(Time.time * 12f) * 0.05f, transform.localScale.y, 1f);
        }
    }
}
