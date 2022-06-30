using System;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Woska
{
    public class ExitMenu : MonoBehaviour
    {
        #region Public Fields
        [SerializeField] private TextMeshProUGUI _infoText;
        [SerializeField] private Image _MasterImage;
        private bool _showMenu = false;
        #endregion

        #region Unity Method

        private void OnEnable()
        {

        }

        private void OnDisable()
        {

        }

        private void Awake()
        {
            _infoText = GetComponentInChildren<TextMeshProUGUI>(true);

        }
        private void Start()
        {
            
        }
        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                ToggleMenu();
            }
            if(!_showMenu)
                return;
            if(!PhotonNetwork.IsConnectedAndReady) 
                return;
            var playersOnline = PhotonNetwork.CountOfPlayers;
            _infoText.text = $"Players online: {playersOnline}";

            _MasterImage.enabled = PhotonNetwork.IsMasterClient;
            
        }

        private void ToggleMenu()
        {
            _showMenu = !_showMenu;
            transform.GetChild(0).gameObject.SetActive(_showMenu);
            transform.GetChild(1).gameObject.SetActive(_showMenu);
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion
    }
}
