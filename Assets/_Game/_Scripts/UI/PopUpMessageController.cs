using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Woska.Core;

namespace Woska
{
    public struct PopUpMessage
    {
        public PopUpMessage(string text)
        {
            Text = text;
            Color = Color.black;
            Time = 1f;
            Position = Vector2.down;
            Scale = 1;
        }
        public PopUpMessage(string text, Color color, float time, Vector2 position, float scale)
        {
            Text = text;
            Color = color;
            Time = time;
            Position = position;
            Scale = scale;
        }
        public string Text { get; }
        public Color Color { get; }
        public float Time { get; }
        public Vector2 Position { get; }
        public float Scale { get; }
    }
    public class PopUpMessageController : Singleton<PopUpMessageController>
    {
        #region Public Fields
        
        [SerializeField] private GameObject popUpPrefab;
        [SerializeField] private Transform[] popupPositions;

        private List<PopUpMessage> allPopups = new List<PopUpMessage>();
        private bool isRunning = false;
        private Coroutine current;
        #endregion

        #region Unity Method
        

        private void Awake()
        {
         
        }
        private void Start()
        {
            
        }
        private void Update()
        {
            
        }

        #endregion

        #region Public Methods
        public void InfoPopUp(string popUpMessage)
        {
            allPopups.Add(new PopUpMessage(popUpMessage, Color.black, 1f, popupPositions[1].position,1));
            if(isRunning) 
                return;

            current = StartCoroutine(InstantiatePopup());
        }
        public void GameInfoPopUp(string popUpMessage)
        {
            allPopups.Add(new PopUpMessage(popUpMessage, Color.black, 1f, popupPositions[0].position, 5));
            if(isRunning) 
                return;

            current = StartCoroutine(InstantiatePopup());
        }

        public void CreatePopUp(string popUpMessage)
        {
            allPopups.Add(new PopUpMessage(popUpMessage, Color.black, 1f, popupPositions[1].position, 1));
            if(isRunning) 
                return;

            current = StartCoroutine(InstantiatePopup());
        }

        public void ClearPopups()
        {
            StopCoroutine(current);
            
            allPopups.Clear();
            isRunning = false;
        }
        #endregion

        #region Private Methods
        IEnumerator InstantiatePopup()
        {
            isRunning = true;
            while (allPopups.Count != 0)
            {
                var currentPopup= allPopups[0];
                Instantiate(popUpPrefab, currentPopup.Position, Quaternion.identity, transform).TryGetComponent(out TextMeshProUGUI textMesh);
                
                textMesh.text = currentPopup.Text;
                textMesh.color = currentPopup.Color;
                textMesh.fontSize *= currentPopup.Scale;
                Destroy(textMesh.gameObject, currentPopup.Time);
                yield return Helpers.GetWait(currentPopup.Time);
                
                allPopups.RemoveAt(0);
            }
            isRunning = false;
        }
        #endregion
    }
}
