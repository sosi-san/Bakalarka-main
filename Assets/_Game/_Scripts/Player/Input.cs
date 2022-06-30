using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Woska
{
    public class Input : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        public InputAction moveInput;
        public InputAction jumpInput;
        public InputAction duckInput;
        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
            
            jumpInput = _playerInputActions.Gameplay.Jump;
            moveInput = _playerInputActions.Gameplay.Movement;
            duckInput = _playerInputActions.Gameplay.Duck;
            if(!GetComponent<PhotonView>().IsMine)
                Destroy(this);
        }
        private void OnEnable()
        {
            MiniGameManager.OnMiniGameStartAction += () => SetActiveAll(true);
            MiniGameManager.MiniGameStopAction += () => SetActiveAll(false);
        }

        private void OnDisable()
        {
            MiniGameManager.OnMiniGameStartAction -= () => SetActiveAll(true);
            MiniGameManager.MiniGameStopAction -= () => SetActiveAll(false);
        }
        public void SetActiveAll(bool active)
        {
            if (active)
            {
                jumpInput.Enable();
                moveInput.Enable();
                duckInput.Enable();
            }
            else
            {
                jumpInput.Disable();
                moveInput.Disable();
                duckInput.Disable();
            }
        }
    }
}