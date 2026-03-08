using UnityEngine;

namespace NeonKolobok.Environment
{
    public class RotatingHazard : MonoBehaviour
    {
        [SerializeField] private float speed = 220f;

        private void Update()
        {
            transform.Rotate(Vector3.forward, speed * Time.deltaTime);
        }
    }
}
