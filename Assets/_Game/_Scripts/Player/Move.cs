using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Woska
{
    [RequireComponent(typeof(Ground))]
    public class Move : MonoBehaviour
    {
        #region Public Fields

        [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
        
        [SerializeField] private bool instantAcceleration;
        [SerializeField, Range(0f, 100f)] private float maxAcceleration = 4f;
        [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 4f;
        
        [SerializeField] private bool instantDeAcceleration;
        [SerializeField, Range(0f, 100f)] private float maxDeAcceleration = 4f;
        [SerializeField, Range(0f, 100f)] private float maxAirDeAcceleration = 4f;

        private Ground _ground;
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;
        private Rigidbody2D _rigidbody2D;
        private Input _input;
        
        private Vector2 _direction;
        private Vector2 _desiredVelocity;
        private Vector2 _velocity;
        
        private float _acceleration;
        private float _maxSpeedChange;
        #endregion

        #region Unity Method

        private void Awake()
        {
            _ground = GetComponent<Ground>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _animator = GetComponentInChildren<Animator>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _input = GetComponent<Input>();
        }
        private void Start()
        {
            SubscribeToActions();
        }
        private void Update()
        {
            _desiredVelocity = _direction * Mathf.Max(maxSpeed, 0f);
        }

        private void FixedUpdate()
        {
            _velocity = _rigidbody2D.velocity;

            if (_desiredVelocity == Vector2.zero)
            {
                _acceleration = _ground.OnGround ? maxDeAcceleration : maxAirDeAcceleration;
                _maxSpeedChange = !instantDeAcceleration ? _acceleration * Time.deltaTime : Single.PositiveInfinity;
            }
            else
            {
                _acceleration = _ground.OnGround ? maxAcceleration : maxAirAcceleration;
                _maxSpeedChange = !instantAcceleration ? _acceleration * Time.deltaTime : Single.PositiveInfinity;
            }
            
            _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);

            _rigidbody2D.velocity = _velocity;


            var animConst = 3;
            _animator.SetFloat("RunSpeed", (Mathf.Abs(_velocity.x)/maxSpeed)*animConst);
            _animator.SetBool("Running", _velocity.x != 0);
            FlipDirection();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void FlipDirection()
        {
            var direction = _direction.x > 0 ? 1 : -1;
            if (_direction == Vector2.zero)
                direction = (int)_spriteRenderer.transform.localScale.x;
            _spriteRenderer.transform.localScale = new Vector2( direction, 1);
        }
        private void MoveInput(InputAction.CallbackContext obj)
        {
            _direction = obj.ReadValue<float>() * Vector2.right;
        }
        private void SubscribeToActions()
        {
            _input.moveInput.performed += MoveInput;
            _input.moveInput.canceled += MoveInput;
        }
        #endregion
    }
}
