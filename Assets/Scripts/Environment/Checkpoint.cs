using NeonKolobok.Core;
using NeonKolobok.Player;
using NeonKolobok.Systems;
using UnityEngine;

namespace NeonKolobok.Environment
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer marker;
        private bool _used;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_used || !other.TryGetComponent<PlayerController>(out _)) return;
            if (GameSession.SelectedMode == GameMode.Hardcore) return;

            FindObjectOfType<RespawnSystem>()?.SetCheckpoint(transform.position + Vector3.up * 1.2f);
            if (marker != null) marker.color = new Color(0.2f, 1f, 0.5f, 1f);
            _used = true;
        }
    }
}
