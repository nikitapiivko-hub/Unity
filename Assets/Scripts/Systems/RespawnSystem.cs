using NeonKolobok.Core;
using NeonKolobok.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NeonKolobok.Systems
{
    public class RespawnSystem : MonoBehaviour
    {
        [SerializeField] private PlayerController player;
        private Vector3 _spawnPoint;
        private int _deaths;
        private bool _hardcore;

        public int Deaths => _deaths;

        public void Initialize(PlayerController controlledPlayer, bool hardcore)
        {
            player = controlledPlayer;
            _spawnPoint = player.transform.position;
            _hardcore = hardcore;
        }

        public void SetCheckpoint(Vector3 point)
        {
            if (_hardcore)
            {
                return;
            }

            _spawnPoint = point;
        }

        public void KillPlayer()
        {
            _deaths++;
            AudioManager.Instance?.PlaySfx(AudioManager.SfxId.Death);
            CameraEffects.Instance?.Shake(0.2f, 0.2f);
            var rb = player.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            player.transform.position = _spawnPoint;
        }

        public void ForceRestart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void CompleteRun(float time)
        {
            GameSession.LastRunTime = time;
            GameSession.LastRunDeaths = _deaths;
            SceneManager.LoadScene("Results");
        }
    }
}
