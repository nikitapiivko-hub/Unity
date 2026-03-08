using NeonKolobok.Player;
using UnityEngine;

namespace NeonKolobok.Environment
{
    public class BoostZone : MonoBehaviour
    {
        [SerializeField] private float boostForce = 20f;

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!other.TryGetComponent<PlayerController>(out var player)) return;
            player.Boost(boostForce);
        }
    }
}
