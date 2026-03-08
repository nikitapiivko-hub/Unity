using NeonKolobok.Player;
using NeonKolobok.Systems;
using UnityEngine;

namespace NeonKolobok.Environment
{
    public class FinishZone : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.TryGetComponent<PlayerController>(out _)) return;
            var timer = FindObjectOfType<TimerSystem>();
            FindObjectOfType<RespawnSystem>()?.CompleteRun(timer != null ? timer.Elapsed : 0f);
        }
    }
}
