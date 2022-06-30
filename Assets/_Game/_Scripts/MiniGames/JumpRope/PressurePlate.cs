using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Woska
{
    public class PressurePlate : MonoBehaviourPun
    {
        #region Public Fields
        private SpriteRenderer _spriteRenderer;
        private Transform _buttonChild;

        [SerializeField] private Color _colorActive;
        [SerializeField] private Color _colorCooldown;
        [SerializeField] private float cooldownTime;
        [SerializeField] private float pushDistance = 0.2f;

        private bool OnCooldown;

        public Action OnActivatedAction;
        #endregion

        #region Unity Method

        private void OnEnable()
        {
            MiniGameManager.OnMiniGameStartAction += StartCooldown;
        }
        private void OnDisable()
        {
            MiniGameManager.OnMiniGameStartAction -= StartCooldown;
        }
        private void Awake()
        {
            _buttonChild = transform.GetChild(0);
            _spriteRenderer = _buttonChild.GetComponent<SpriteRenderer>();
        }
        private void Start()
        {
            _spriteRenderer.color = _colorCooldown;
        }
        private void Update()
        {
            
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void StartCooldown()
        {
            StartCoroutine(Cooldown());
        }

        private IEnumerator Cooldown()
        {
            OnCooldown = true;
            _spriteRenderer.color = _colorCooldown;
            _buttonChild.position -= _buttonChild.right.normalized * pushDistance;
            
            yield return Helpers.GetWait(cooldownTime);
            
            _spriteRenderer.color = _colorActive;
            _buttonChild.position += _buttonChild.right.normalized * pushDistance;
            OnCooldown = false;
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(!PhotonNetwork.IsMasterClient) 
                return;
            if(OnCooldown) 
                return;
            if (col.CompareTag("Player"))
            {
                OnActivatedAction?.Invoke();
                photonView.RPC("RPC_UpdateState", RpcTarget.All);
            }
        }
        [PunRPC]
        private void RPC_UpdateState()
        {
            StartCoroutine(Cooldown());
        }

        #endregion
    }
}
