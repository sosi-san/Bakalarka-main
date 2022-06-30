using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace Woska
{
    public class HelpBlock : MonoBehaviourPun
    {
        #region Public Fields

        private BoxCollider2D _boxCollider2D;
        private Rigidbody2D _rigidbody2D;
        private SpriteRenderer _spriteRenderer;
        private bool dissolving;
        private float dissolveTime = 0.5f;
        private float coolDownTime = 5f;

        private Vector3 _startPosition;
        
        #endregion

        #region Unity Method

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _startPosition = transform.position;
        }
        private void Start()
        {
            
        }
        private void Update()
        {
            
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private IEnumerator Dissolve()
        {
            dissolving = true;
            yield return Helpers.GetWait(dissolveTime);
            _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
            _boxCollider2D.enabled = false;
            //_spriteRenderer.enabled = false;
            if(PhotonNetwork.IsMasterClient)
                photonView.RPC("RPC_Cooldown", RpcTarget.All);
                
        }
        private IEnumerator CoolDown()
        {
            yield return Helpers.GetWait(coolDownTime);
            _boxCollider2D.enabled = true;
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            //_spriteRenderer.enabled = true;
            transform.position = _startPosition;
            dissolving = false;

        }
        private void OnCollisionEnter2D(Collision2D col)
        {
            if(!PhotonNetwork.IsMasterClient) 
                return;
            if(dissolving)
                return;
            if(!col.gameObject.CompareTag("Player"))
                return;
            photonView.RPC("RPC_UpdateState", RpcTarget.All);
        }
        [PunRPC]
        private void RPC_UpdateState()
        {
            StartCoroutine(nameof(Dissolve));
        }
        [PunRPC]
        private void RPC_Cooldown()
        {
            StartCoroutine(nameof(CoolDown));
        }
        #endregion
    }
}
