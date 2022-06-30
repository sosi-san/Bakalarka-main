using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Woska.Core;

namespace Woska
{
    [RequireComponent(typeof(PhotonView))]
    public abstract class MiniGameManager : MonoBehaviourPun
    {
        #region Public Fields

        [SerializeField] protected float gameTime = 10f;
        [SerializeField] protected GameObject playerPrefab;
        [SerializeField] protected Transform[] playerSpawnPoints;
        
        public static Action OnMiniGameStartAction;
        public static Action MiniGameStopAction;
        public Action OnMiniGameEndAction;
        
        public static Action<float> OnGameTickAction;

        public GameObject PlayerPrefab => playerPrefab;
        public Transform[] SpawnPositions => playerSpawnPoints;

        protected int _playersLeft;
        
        protected static List<GameObject> allWheels = new List<GameObject>();

        protected List<Player> _playersAlive = new List<Player>();

        private bool miniGameEnd;
        
        public string GameName;
        [TextArea()] public string description;
        
        #endregion

        #region Unity Method

        protected virtual void OnEnable()
        {
            GameController.OnMiniGameSceneLoadedAction += SetUpGame;
            
            GameController.OnAllPlayersReadyAction += StartCountdown;
            //OnPlayerDied += CheckEndGame;
        }

        protected virtual void OnDisable()
        {
            GameController.OnMiniGameSceneLoadedAction -= SetUpGame;
            
            GameController.OnAllPlayersReadyAction -= StartCountdown;
            //OnPlayerDied -= CheckEndGame;
        }
        #endregion

        #region Public Methods
        #endregion
        #region Private Methods
        /*
         * Is called when scene is loaded
         */
        protected virtual void SetUpGame()
        {
            var ownerIndex = PhotonNetwork.LocalPlayer.GetPlayerNumber();
            PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnPositions[ownerIndex].position, Quaternion.identity);

            _playersAlive = PlayerNumbering.SortedPlayers.ToList();
        }
        /*
         * Is called when all players are ready to start
         */
        private void StartCountdown()
        {
            _playersLeft = PlayerManager.allManagers.Count;
            
            StartCoroutine(CountDown());
            OnGameTickAction?.Invoke(gameTime);
        }
        private IEnumerator CountDown()
        {
            PopUpMessageController.Instance.ClearPopups();
            
            PopUpMessageController.Instance.GameInfoPopUp("3");
            PopUpMessageController.Instance.GameInfoPopUp("2");
            PopUpMessageController.Instance.GameInfoPopUp("1");
            
            yield return Helpers.GetWait(3f);
            
            OnMiniGameStartAction?.Invoke();
            GameLoop();
        }

        protected virtual void GameLoop(){}

        public void PlayerDied(Player player)
        {
            if (_playersLeft > 1)
            {
                _playersLeft--;
                _playersAlive.Remove(player);
            }

            if (_playersLeft != 1) return;

            if(miniGameEnd) return;
            
            miniGameEnd = true;
            
            var winner = _playersAlive[0];
            winner.AddScore(GameSettings.Instance.ScoreForWin);
            PopUpMessageController.Instance.GameInfoPopUp(winner.NickName + " is winner");

            StartCoroutine(GameEnd());

        }
        protected IEnumerator GameEnd()
        {
            //Stop mini game
            CleanUp();
            PopUpMessageController.Instance.GameInfoPopUp("Game End");
            yield return Helpers.GetWait(3f);
            OnMiniGameEndAction?.Invoke();
        }
        protected virtual void CleanUp() {}
        /*
        protected virtual void CheckEndGame()
        {
            _playersLeft--;
            if (_playersLeft <= 1)
            {
                //Destroys all stuff that can kill player
                OnGameTickAction?.Invoke(gameTime);
                if (PhotonNetwork.IsMasterClient)
                {
                    DestroyAllObjects();
                }
                //gameTime = 0;
                StartCoroutine(GameEnd());
            }
        }
*/
        #endregion
    }
}
