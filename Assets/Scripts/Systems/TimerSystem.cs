using UnityEngine;

namespace NeonKolobok.Systems
{
    public class TimerSystem : MonoBehaviour
    {
        public float Elapsed { get; private set; }

        private void Update()
        {
            Elapsed += Time.deltaTime;
        }
    }
}
