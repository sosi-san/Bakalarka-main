using Photon.Pun;
using Photon.Pun.UtilityScripts;
using TMPro;
using UnityEngine;

namespace Woska
{
    public class PlayerSlot : MonoBehaviour
    {
        #region Public Fields
        private TextMeshProUGUI _text;
        [SerializeField] private int forPlayerID;
        #endregion

        #region Unity Method
        private void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += UpdateSlot;
            
            //GameController.OnRoomStartAction += UpdateSlot;
        }

        private void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= UpdateSlot;
            
            //GameController.OnRoomStartAction -= UpdateSlot;
        }
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
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

        private void UpdateSlot()
        {
            if (forPlayerID < PlayerNumbering.SortedPlayers.Length)
                _text.text = PlayerNumbering.SortedPlayers[forPlayerID].NickName;
            else
                _text.text = "EMPTY";
        }
        #endregion
    }
}
