using System;
using TMPro;
using UnityEngine;

namespace Woska
{
    public class MiniGameTimer : MonoBehaviour
    {
        #region Public Fields

        private TextMeshProUGUI _timerText;
        #endregion

        #region Unity Method

        private void OnEnable()
        {
            MiniGameManager.OnGameTickAction += OnGameTickAction;
        }
        private void OnDisable()
        {
            MiniGameManager.OnGameTickAction -= OnGameTickAction;
        }
        private void Awake()
        {
            _timerText = GetComponent<TextMeshProUGUI>();
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
        private void OnGameTickAction(float obj)
        {
            _timerText.SetText(obj.ToString());
        }
        #endregion
    }
}
