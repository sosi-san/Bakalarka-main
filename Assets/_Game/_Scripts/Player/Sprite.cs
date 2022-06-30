using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

namespace Woska
{
    public class Sprite : MonoBehaviour
    {
        #region Public Fields
        private TextMeshPro _playerName;
        private SpriteRenderer _spriteRenderer;
        #endregion

        #region Unity Method

        private void Awake()
        {
            _playerName = GetComponentInChildren<TextMeshPro>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
            _spriteRenderer.color = playerManager.PlayerInfo.playerColor;
        }
        #endregion
    }
}
