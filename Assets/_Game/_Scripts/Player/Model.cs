using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

namespace Woska
{
    public class Model : MonoBehaviour
    {
        #region Public Fields
        private TextMeshPro _playerName;
        private MeshRenderer _spriteRenderer;
        #endregion

        #region Unity Method

        private void Awake()
        {
            _playerName = GetComponentInChildren<TextMeshPro>();
            _spriteRenderer = GetComponentInChildren<MeshRenderer>();
        }
        private void Start()
        {
            Init();
        }
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void Init()
        {
            var photonView = GetComponent<PhotonView>();
            var idOfOwner = photonView.Owner.GetPlayerNumber();
            var playerManager = PlayerManager.allManagers.Find(m => m.PlayerInfo.playerID == idOfOwner);
            _playerName.text = playerManager.PlayerInfo.playerName;
            if(photonView.IsMine) 
                _playerName.color = playerManager.PlayerInfo.playerColor;
            _spriteRenderer.material = playerManager.PlayerInfo.playerMaterial;
        }
        #endregion
    }
}
