using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Woska
{
    public class PlayerNickNameInput : MonoBehaviour
    {
        #region Public Fields

        public static Action NameChangedAction;
        
        #endregion
        

        #region Unity Method

        private void OnEnable()
        {

        }
        private void Awake()
        {

        }
        private void Start()
        {
            TryGetComponent(out TMP_InputField inputField);
            inputField.onEndEdit.AddListener(delegate{CheckInputValue(inputField);});
            inputField.text = PlayerPrefs.GetString("NickName");
        }
        private void Update()
        {
            
        }

        #endregion

        #region Public Methods
        private void CheckInputValue(TMP_InputField input)
        {
            if (input.text.Length > 0) 
            {
                PlayerPrefs.SetString("NickName", input.text.ToUpper());
                PopUpMessageController.Instance.InfoPopUp("Name changed to: " + input.text);
            }
            else if (input.text.Length == 0) 
            {
                PopUpMessageController.Instance.InfoPopUp("No value");
                input.text = PlayerPrefs.GetString("NickName");
            }
            NameChangedAction?.Invoke();
        }
        #endregion

        #region Private Methods
        
        #endregion
    }
}
