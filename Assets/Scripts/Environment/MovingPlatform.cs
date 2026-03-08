using UnityEngine;

namespace NeonKolobok.Environment
{
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private Vector3 direction = Vector3.right;
        [SerializeField] private float distance = 3f;
        [SerializeField] private float speed = 1.5f;

        private Vector3 _start;

        private void Start() => _start = transform.position;

        private void Update()
        {
            var t = (Mathf.Sin(Time.time * speed) + 1f) * 0.5f;
            transform.position = _start + direction.normalized * (t * distance);
        }
    }
}
