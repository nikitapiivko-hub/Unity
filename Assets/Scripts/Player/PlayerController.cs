using NeonKolobok.Systems;
using UnityEngine;

namespace NeonKolobok.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float moveSpeed = 8f;
        [SerializeField] private float jumpForce = 14f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float coyoteTime = 0.1f;

        private Rigidbody2D _rb;
        private float _coyoteCounter;
        private bool _jumpQueued;

        public RespawnSystem RespawnSystem { get; set; }

        public void Configure(Transform check, LayerMask mask)
        {
            groundCheck = check;
            groundMask = mask;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpQueued = true;
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                RespawnSystem?.ForceRestart();
            }
        }

        private void FixedUpdate()
        {
            var horizontal = Input.GetAxisRaw("Horizontal");
            _rb.velocity = new Vector2(horizontal * moveSpeed, _rb.velocity.y);

            var grounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundMask);
            _coyoteCounter = grounded ? coyoteTime : _coyoteCounter - Time.fixedDeltaTime;

            if (_jumpQueued && _coyoteCounter > 0f)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
                _jumpQueued = false;
                _coyoteCounter = 0f;
                AudioManager.Instance?.PlaySfx(AudioManager.SfxId.Jump);
            }

            _jumpQueued = false;
        }

        public void Boost(float force)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, force);
        }
    }
}
