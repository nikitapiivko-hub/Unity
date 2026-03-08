using UnityEngine;

namespace NeonKolobok.Systems
{
    public class CameraEffects : MonoBehaviour
    {
        public static CameraEffects Instance { get; private set; }

        private Vector3 _origin;
        private float _shakeTimer;
        private float _shakeMagnitude;

        private void Awake()
        {
            Instance = this;
            _origin = transform.localPosition;
        }

        public void Shake(float duration, float magnitude)
        {
            _shakeTimer = duration;
            _shakeMagnitude = magnitude;
        }

        private void LateUpdate()
        {
            if (_shakeTimer <= 0f)
            {
                transform.localPosition = _origin;
                return;
            }

            _shakeTimer -= Time.deltaTime;
            transform.localPosition = _origin + (Vector3)Random.insideUnitCircle * _shakeMagnitude;
        }
    }
}
