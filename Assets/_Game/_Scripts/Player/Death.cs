using System;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Woska.Bakalarka;

namespace Woska
{
    public class Death : MonoBehaviour
    {
        #region Public Fields
        [SerializeField] private LayerMask instantKillLayers;

        private Input _input;
        private BoxCollider2D _boxCollider2D;
        private Action<Player> OnPlayerDied;
        #endregion

        #region Unity Method

        private void Awake()
        {
            _input = GetComponent<Input>();
            _boxCollider2D = GetComponentInChildren<BoxCollider2D>();
        }
        #endregion

        #region Public Methods
        public void Kill()
        {
            var photonView = GetComponent<PhotonView>();
            GetComponent<Rigidbody2D>().AddForce(Vector2.up*2, ForceMode2D.Impulse);
            photonView.RPC("RPC_PlayerDead", RpcTarget.All, photonView.Owner);
        }
        #endregion
        #region Private Methods

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!PhotonNetwork.IsMasterClient)
                return;
            var otherGameObject = other.gameObject;
            if (!otherGameObject.IsOnLayer(instantKillLayers))
                return;
            Kill();
        }
        [PunRPC]
        protected void RPC_PlayerDead(Player player)
        {
            FindObjectOfType<MiniGameManager>().PlayerDied(player);
            _boxCollider2D.enabled = false;
            _input.SetActiveAll(false);
        }
        #endregion
    }
}
