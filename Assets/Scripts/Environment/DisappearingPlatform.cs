using UnityEngine;

namespace NeonKolobok.Environment
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class DisappearingPlatform : MonoBehaviour
    {
        [SerializeField] private float visibleDuration = 1.5f;
        [SerializeField] private float hiddenDuration = 1.0f;

        private Collider2D _collider;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            var cycle = visibleDuration + hiddenDuration;
            var t = Time.time % cycle;
            var visible = t <= visibleDuration;
            _collider.enabled = visible;
            _renderer.enabled = visible;
        }
    }
}
