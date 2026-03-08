using NeonKolobok.Player;
using NeonKolobok.Systems;
using UnityEngine;

namespace NeonKolobok.Environment
{
    public class Hazard : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<PlayerController>(out _)) return;
            FindObjectOfType<RespawnSystem>()?.KillPlayer();
        }
    }
}
