using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Woska.Core;
using Random = UnityEngine.Random;

namespace Woska
{
    public class NetworkController : MonoBehaviourPunCallbacks
    {
        #region Actions
        public static Action ConnectedToServerAction;
        
        public static Action OnRoomJoinedAction;
        public static Action OnRoomLeftAction;
        
        public static Action OnPlayerLeftRoomAction;
        

        #endregion
        #region Public Fields
        private bool _initialConnection = false;
        
        private string joinRoomCode = String.Empty;
        #endregion

        #region Unity Method

        public override void OnEnable()
        {
            base.OnEnable();
            PlayerNickNameInput.NameChangedAction += SetPlayerName;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PlayerNickNameInput.NameChangedAction -= SetPlayerName;
        }
        private void Awake()
        {
            
        }
        private void Start()
        {
            Connect();
        }

        private void Connect()
        {
            if (PhotonNetwork.IsConnected) return;
            
            PhotonNetwork.GameVersion = GameSettings.Instance.GameVersion;
            SetPlayerName();
            PhotonNetwork.ConnectUsingSettings();
        }
        private void Update()
        {
            
        }

        #endregion

        #region Public Methods
        public void CreateRoom()
        {
            if(!PhotonNetwork.IsConnectedAndReady)
                return;
                
            if(PhotonNetwork.InRoom) 
                return;

            PhotonNetwork.CreateRoom(Helpers.RandomString(4), new RoomOptions() {IsOpen = true, MaxPlayers = (byte)GameSettings.Instance.MaximumPlayers, IsVisible = true});
        }

        public void SetRoomCode(string roomCode)
        {
            joinRoomCode = roomCode;
        }
        public void JoinRoom()
        {
            if(!PhotonNetwork.IsConnectedAndReady)
                return;
            
            if(PhotonNetwork.InRoom) 
                return;

            joinRoomCode = GameObject.FindWithTag("RoomCode").GetComponent<TMP_InputField>().text.ToUpper();

            if (joinRoomCode.Equals(String.Empty))
                PhotonNetwork.JoinRandomRoom();
            else
                PhotonNetwork.JoinRoom(joinRoomCode);
        }
        public void LeaveRoom()
        {
            if(!PhotonNetwork.InRoom || PhotonNetwork.NetworkClientState == ClientState.Leaving) 
                return;
            
            PhotonNetwork.LeaveRoom();
        }
        #endregion

        #region Private Methods
        private void SetPlayerName()
        {
            var name = "Player_" + Random.Range(0, 99);
            if (PlayerPrefs.HasKey("NickName"))
            {
                name = PlayerPrefs.GetString("NickName");
            }
            else
            {
                PlayerPrefs.SetString("NickName", name);
            }
            PhotonNetwork.NickName = name;
        }
        #endregion

        #region Network CallBacks

        /*
         * General callbacks
         */
        public override void OnConnectedToMaster()
        {
            if(_initialConnection) 
                return;
            PhotonNetwork.JoinLobby();
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.Connected);
        }
        public override void OnJoinedLobby()
        {
            PopUpMessageController.Instance.InfoPopUp("Joined lobby");
            ConnectedToServerAction?.Invoke();
            _initialConnection = true;
        }
        public override void OnDisconnected(DisconnectCause cause)
        {
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.Disconnected);
        }
        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            LeaveRoom();
        }
        #endregion
        
        #region MatchmakingCallbacks
        public override void OnCreatedRoom()
        {
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.RoomCreated);
        }
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.FailedToCreatedRoom);
        }
        public override void OnJoinedRoom()
        {
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.RoomJoined);
            OnRoomJoinedAction?.Invoke();
        }
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.FailedToJoinedRoom);
        }
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.FailedToJoinedRoom);
        }
        public override void OnLeftRoom()
        {
            PopUpMessageController.Instance.InfoPopUp(NetworkLog.RoomLeft);
            OnRoomLeftAction?.Invoke();
        }
        #endregion

        #region InRoomCallbacks

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            PopUpMessageController.Instance.InfoPopUp(newPlayer.NickName + NetworkLog.PlayerJoinedRoom);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            PopUpMessageController.Instance.InfoPopUp(otherPlayer.NickName + NetworkLog.PlayerLeftRoom);
            OnPlayerLeftRoomAction?.Invoke();
        }

        #endregion
    }

    public class NetworkLog
    {
        public static string Connected => "Connected to server";
        public static string Disconnected => "Disconnected from server";
        
        public static string RoomCreated => "Room was created";
        public static string RoomJoined => "Room entered";
        public static string PlayerJoinedRoom => " has entered";
        public static string FailedToCreatedRoom => "Room was not created";
        public static string FailedToJoinedRoom => "Room was not joined";
        public static string RoomLeft => "Room left";
        public static string PlayerLeftRoom => " has left";
    }
}
