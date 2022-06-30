using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Woska
{
    public class Duck : MonoBehaviourPun
    {
        [SerializeField] private float duckScaleFactor = 0.5f;

        private Ground _ground;
        private Input _input;
        
        public bool isDucking { get; private set; }

        private void Awake()
        {
            _ground = GetComponent<Ground>();
            _input = GetComponent<Input>();
        }

        private void OnDisable()
        {
            _input.duckInput.performed -= ChangeScale;
            _input.duckInput.canceled -= ChangeScale;
        }

        private void Start()
        {
            SubscribeToActions();
        }
        private void SubscribeToActions()
        {
            _input.duckInput.performed += ChangeScale;
            _input.duckInput.canceled += ChangeScale;
        }

        private void ChangeScale(InputAction.CallbackContext obj)
        {
            if (obj.phase == InputActionPhase.Canceled)
            {
                transform.localScale = Vector3.one;
                isDucking = false;
            }
            else if (obj.phase == InputActionPhase.Performed)
            {
                transform.localScale = Vector3.one * duckScaleFactor;
                isDucking = true;
            }
        }
    }
}