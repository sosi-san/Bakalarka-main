using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Woska
{
    public enum CanvasType
    {
        SPLASH_SCREEN,
        MENU,
        JOIN_MENU,
        ROOM,
        GAME_INFO,
        LOADING,
        SCORE,
        MINI_GAME
    }
    public class CanvasManager : MonoBehaviour
    {
        #region Public Fields
        #endregion

        private List<CanvasController> _canvases;

        private CanvasController _current;

        [SerializeField]private TextMeshProUGUI gameTitle;
        [SerializeField]private TextMeshProUGUI gameControlls;
        
        #region Unity Method

        private void OnEnable()
        {
            NetworkController.ConnectedToServerAction += () => SwitchCanvas(CanvasType.MENU);
            NetworkController.OnRoomJoinedAction += () => SwitchCanvas(CanvasType.ROOM);
            NetworkController.OnRoomLeftAction += () => SwitchCanvas(CanvasType.JOIN_MENU);
            
            GameController.OnLoadingMiniGameAction += () => SwitchCanvas(CanvasType.LOADING);
            GameController.OnMiniGameSceneLoadedAction += MiniGameInfo;
            
            GameController.ShowScoreAction += () => SwitchCanvas(CanvasType.SCORE);
            GameController.OnAllPlayersReadyAction += () => SwitchCanvas(CanvasType.MINI_GAME);
            
            GameController.AllMiniGamesEndAction += () => SwitchCanvas(CanvasType.JOIN_MENU);
        }

        private void OnDisable()
        {
            NetworkController.ConnectedToServerAction -= () => SwitchCanvas(CanvasType.MENU);
            NetworkController.OnRoomJoinedAction -= () => SwitchCanvas(CanvasType.ROOM);
            NetworkController.OnRoomLeftAction -= () => SwitchCanvas(CanvasType.JOIN_MENU);
            
            GameController.OnLoadingMiniGameAction -= () => SwitchCanvas(CanvasType.LOADING);
            GameController.OnMiniGameSceneLoadedAction -= MiniGameInfo;
            
            GameController.ShowScoreAction -= () => SwitchCanvas(CanvasType.SCORE);
            GameController.OnAllPlayersReadyAction -= () => SwitchCanvas(CanvasType.MINI_GAME);
            
            GameController.AllMiniGamesEndAction -= () => SwitchCanvas(CanvasType.JOIN_MENU);
        }
        private void Start()
        {
            GetCanvases();
            _canvases.ForEach(x => x.gameObject.SetActive(false));
            SwitchCanvas(CanvasType.SPLASH_SCREEN);
        }
        #endregion

        #region Public Methods

        private void Update()
        {
            if(_current == null) return;
            
            if (_current.CanvasType == CanvasType.MENU)
            {
                if (Keyboard.current.anyKey.wasPressedThisFrame && !Keyboard.current.escapeKey.wasPressedThisFrame)
                {
                    SwitchCanvas(CanvasType.JOIN_MENU);
                }
            }
        }

        #endregion

        #region Private Methods
        private void GetCanvases()
        {
            _canvases = GetComponentsInChildren<CanvasController>().ToList();
        }
        private void SwitchCanvas(CanvasType canvasType)
        {
            if(_current != null)
                _current.gameObject.SetActive(false);
            _current = _canvases.Find(x => x.CanvasType == canvasType);
            _current.gameObject.SetActive(true);
        }

        private void MiniGameInfo()
        {
            var manager = FindObjectOfType<MiniGameManager>();
            
            SwitchCanvas(CanvasType.GAME_INFO);
            gameControlls.text = manager.description.ToString();
            gameTitle.text = manager.GameName.ToString();
        }
        #endregion
    }
}
