using System;
using Photon.Pun;
using TMPro;
using UnityEngine;

namespace Woska
{
    public class RoomCode : MonoBehaviour
    {
        #region Public Fields

        private TextMeshProUGUI _textMeshProUGUI;
        #endregion

        #region Unity Method
        

        private void OnEnable()
        {
            NetworkController.OnRoomJoinedAction += RoomJoined;
        }
        private void OnDisable()
        {
            NetworkController.OnRoomJoinedAction -= RoomJoined;
        }

        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            NetworkController.OnRoomJoinedAction += RoomJoined;
        }

        private void RoomJoined()
        {
            _textMeshProUGUI.text = PhotonNetwork.CurrentRoom.Name;
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
