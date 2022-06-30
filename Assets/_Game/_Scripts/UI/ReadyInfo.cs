using System;
using TMPro;
using UnityEngine;

namespace Woska
{
    public class ReadyInfo : MonoBehaviour
    {
        #region Public Fields

        [SerializeField] private Color _ready;
        [SerializeField] private Color _unready;
        private TextMeshProUGUI _readyStatus;
        #endregion

        #region Unity Method

        private void OnEnable()
        {
            GameController.ReadyToggleAction += ToggleStatus;
        }
        
        private void OnDisable()
        {
            GameController.ReadyToggleAction -= ToggleStatus;
        }

        private void Awake()
        {
            _readyStatus = GetComponent<TextMeshProUGUI>();
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
        private void ToggleStatus(bool obj)
        {
            _readyStatus.color = obj ? _ready : _unready;
        }
        #endregion
    }
}
