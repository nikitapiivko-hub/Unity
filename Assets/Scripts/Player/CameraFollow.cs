using UnityEngine;

namespace NeonKolobok.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private float smooth = 6f;
        [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -10f);

        public void SetTarget(Transform followTarget)
        {
            target = followTarget;
        }

        private void LateUpdate()
        {
            if (target == null) return;
            transform.position = Vector3.Lerp(transform.position, target.position + offset, Time.deltaTime * smooth);
        }
    }
}
