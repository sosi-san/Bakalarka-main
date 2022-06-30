using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Woska
{
    [RequireComponent(typeof(Ground))]
    public class Jump : MonoBehaviourPun
    {
        #region Public Fields
        
        [SerializeField, Range(0f, 15f)] private float jumpHeight = 2f;
        [SerializeField, Range(0f, 15f)] private float maxAirJumps = 1f;
        
        [SerializeField, Range(0f, 15f)] private float downMultiplier = 3f;
        [SerializeField, Range(0f, 15f)] private float upMultiplier = 2f;
        
        [SerializeField, Range(0f, 0.5f)] private float jumpBufferTime = 0.1f;
        
        private Ground _ground;
        private Duck _duck;
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private Input _input;

        private Vector2 _velocity;
        
        private int _jumpNumber;
        
        private float _defaultGravityScale;
        
        private bool _jumpButtonPressed;

        private float _lastJumpPressedTime = 0f;
        
        private bool BufferedJump => Time.time <= _lastJumpPressedTime + jumpBufferTime;


        #endregion

        #region Unity Method

        private void Awake()
        {
            _ground = GetComponent<Ground>();
            _animator = GetComponentInChildren<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _duck = GetComponent<Duck>();
            _input = GetComponent<Input>();
        }
        private void Start()
        {
            _defaultGravityScale = _rigidbody2D.gravityScale;
            
            SubscribeToActions();
        }
        private void FixedUpdate()
        {
            if(!photonView.IsMine) return;
            
            var onGround = _ground.OnGround;
            _velocity = _rigidbody2D.velocity;
            
            if (onGround)
            {
                _jumpNumber = 0;
            }

            if (BufferedJump)
            {
                MakeJump();
            }

            if(onGround)
            {
                _rigidbody2D.gravityScale = _defaultGravityScale;
            }
            if (_rigidbody2D.velocity.y > 0)
            {
                _rigidbody2D.gravityScale = upMultiplier;
            }
            if (_rigidbody2D.velocity.y < 0 || _rigidbody2D.velocity.y > 0 && !_jumpButtonPressed)
            {
                _rigidbody2D.gravityScale = downMultiplier;

            }
            
            _rigidbody2D.velocity = _velocity;
            if(onGround)
            {
                _animator.SetBool("Jumping", false);
                _animator.SetBool("Falling", false);
            }
            else
            {
                _animator.SetBool("Jumping", _velocity.y > 0);
                _animator.SetBool("Falling", _velocity.y < 0);
            }
        }

        #endregion

        #region Public Methods
        
        #endregion

        #region Private Methods
        

        private void MakeJump()
        {
            // Also check for coyote time and jump buffering
            
            if(_duck != null && _duck.isDucking) return;
            
            if (_ground.OnGround || _jumpNumber < maxAirJumps)
            {
                _jumpNumber++;

                var jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
                _velocity.y = 0f;

                if (_velocity.y > 0f)
                {
                    jumpSpeed = Mathf.Max(jumpSpeed - _velocity.y, 0f);
                }
                _velocity.y += jumpSpeed;
            }
        }
        private void JumpInput(InputAction.CallbackContext obj)
        {
            if (obj.performed)
            {
                _jumpButtonPressed = true;
                _lastJumpPressedTime = Time.time;
            }
            else if (obj.canceled)
            {
                _jumpButtonPressed = false;
            }
        }
        private void SubscribeToActions()
        {
            _input.jumpInput.performed += JumpInput;
            _input.jumpInput.canceled += JumpInput;
        }
        #endregion
    }
}
