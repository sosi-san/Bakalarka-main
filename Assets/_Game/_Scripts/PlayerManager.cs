using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace Woska
{
    public class PlayerManager : MonoBehaviourPun
    {
        #region Public Fields
        public static List<PlayerManager> allManagers = new List<PlayerManager>();
        public Player Owner;
        public PlayerInfo PlayerInfo;
        #endregion

        #region Unity Method

        private void OnDestroy()
        {
            allManagers.Remove(this);
        }

        private void Awake()
        {
            allManagers.Add(this);
            PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayerID;
            PlayerInfo = new PlayerInfo();
            Owner = photonView.Owner;
            Owner.SetScore(0);
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
        private void UpdatePlayerID()
        {
            if(Owner.GetPlayerNumber() == -1) 
                return;
            
            PlayerInfo.playerID = Owner.GetPlayerNumber();
            PlayerInfo.playerName = Owner.NickName;
            PlayerInfo.playerColor = GameSettings.Instance.PlayerColors[PlayerInfo.playerID];
            PlayerInfo.playerMaterial = GameSettings.Instance.PlayerMaterials[PlayerInfo.playerID];
            
        }
        #endregion
    }
    [Serializable]
    public class PlayerInfo
    {
        public Color playerColor;
        public Material playerMaterial;
        public int playerID;
        public string playerName = "";
        public int score;

        public PlayerInfo()
        {
            
        }
    }
}
