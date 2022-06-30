using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Woska.Bakalarka;


namespace Woska
{
    public class GameController : MonoBehaviourPun
    {
        #region Public Fields
        public static Action ShowScoreAction;
        public static Action<bool> ReadyToggleAction;
        public static Action OnAllPlayersReadyAction;
        public static Action OnMiniGameSceneLoadedAction;
        public static Action AllMiniGamesEndAction;
        public static Action OnLoadingMiniGameAction;
        
        private bool localPlayerReady = false;
        private int playersInRoom;
        private int playersReady = 0;
        private MiniGameManager currentManager;
        private int currentMiniGameSceneIndex = -1;
        
        private Scene? currentMiniGameScene = null;
        private Scene? nextMiniGameScene = null;
        private int nextMiniGameSceneIndex;

        
        public PlayerManager localPlayerManager;
        private bool GameInProgress = false;
        private bool forcedMiniGameOn = false;
        private int forcedMiniGame = 0;

        private int numberOfGamesLeft;

        private bool WatingForReady;
        private int miniGamesRounds;
        private bool allPlayersReady => playersReady == playersInRoom;
        private bool NoMiniGamesLeft => numberOfGamesLeft == 0;
        #endregion

        #region Unity Method

        private void OnEnable()
        {
            NetworkController.OnRoomJoinedAction += JoinedRoomAction;
            NetworkController.OnPlayerLeftRoomAction += PlayerLeftGameAction;

        }
        private void OnDisable()
        {
            NetworkController.OnRoomJoinedAction -= JoinedRoomAction;
            NetworkController.OnPlayerLeftRoomAction -= PlayerLeftGameAction;

        }
        private void Update()
        {
            if(!WatingForReady) 
                return;
            
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                localPlayerReady = !localPlayerReady;
                ReadyToggleAction?.Invoke(localPlayerReady);
                photonView.RPC("RPC_IsPlayerReady",RpcTarget.MasterClient,localPlayerReady);
            }
        }

        #endregion

        #region Public Methods
        #endregion

        #region InRoomMethods
        private void JoinedRoomAction()
        {
            PhotonNetwork.Instantiate("PhotonPlayer", Vector3.zero, Quaternion.identity).TryGetComponent(out PlayerManager manager);
            localPlayerManager = manager;
        }
        /*
         * Called when start button in room is pressed
         */
        public void StartGame()
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= GameSettings.Instance.MinimumPlayers)
            {
                forcedMiniGame = -1;
                forcedMiniGameOn = false;
                GameInProgress = true;
                photonView.RPC("RPC_StartMiniGamesRounds",RpcTarget.AllViaServer);
            }
        }
        public void StartGameForced(int forcedGame)
        {
            if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= GameSettings.Instance.MinimumPlayers)
            {
                forcedMiniGame = forcedGame;
                forcedMiniGameOn = true;
                photonView.RPC("RPC_StartMiniGamesRounds",RpcTarget.AllViaServer);
            }
        }
        private int ChooseMiniGame()
        {
            if (forcedMiniGameOn)
                return forcedMiniGame;
            return (int)GameSettings.Instance.AvalibleMinigames.RandomItem();
        }

        private void PlayerLeftGameAction()
        {
            if(GameInProgress == false) 
                return;
            GameInProgress = false;
            StartCoroutine(nameof(CancelGame));
        }
        #endregion

        #region MiniGameLoading
        private IEnumerator LoadMiniGame()
        {
            OnLoadingMiniGameAction?.Invoke();

            ResetOnRound();
            //Debug.Log(numberOfGamesLeft+"/"+GameSettings.Instance.RoundsToPlay);
            numberOfGamesLeft--;

            yield return StartCoroutine(nameof(UnloadMiniSceneGame));
            
            currentMiniGameScene = nextMiniGameScene;
            currentMiniGameSceneIndex = nextMiniGameSceneIndex;
            
            nextMiniGameScene = null;
            nextMiniGameSceneIndex = -1;
            
            yield return StartCoroutine(nameof(LoadMiniGameScene));
            
            
            /*
             * Scene is loaded on local player
            */

            currentManager = FindObjectOfType<MiniGameManager>();
            
            currentManager.OnMiniGameEndAction += OnMiniGameEnd;
            
            // Artificial waiting time to show loading screen
            yield return Helpers.GetWait(0.5f);

            WatingForReady = true;
            OnMiniGameSceneLoadedAction?.Invoke();
            
            ReadyToggleAction?.Invoke(false);
        }

        private IEnumerator CancelGame()
        {
            yield return StartCoroutine(nameof(UnloadMiniSceneGame));
            App.Instance.NetworkController.LeaveRoom();
        }
        private IEnumerator AllRoundsFinished()
        {
            yield return StartCoroutine(nameof(UnloadMiniSceneGame));
            ShowScoreAction?.Invoke();
            GameInProgress = false;
            yield return Helpers.GetWait(5f);
            App.Instance.NetworkController.LeaveRoom();
        }
        private IEnumerator LoadMiniGameScene()
        {
            if (currentMiniGameScene != null)
            {
                yield return SceneManager.LoadSceneAsync(currentMiniGameSceneIndex, LoadSceneMode.Additive);
                
                SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(currentMiniGameSceneIndex));
            }
        }
        private IEnumerator UnloadMiniSceneGame()
        {
            if (currentMiniGameScene != null)
            {
                currentManager.OnMiniGameEndAction -= OnMiniGameEnd;
                currentManager = null;
                yield return SceneManager.UnloadSceneAsync(currentMiniGameSceneIndex);
            }
        }
        private void OnMiniGameEnd()
        {
            if(!PhotonNetwork.IsMasterClient) 
                return;

            if (NoMiniGamesLeft)
            {
                photonView.RPC("RPC_AllRoundsFinished",RpcTarget.All);
            }
            else
            {
                photonView.RPC("RPC_LoadMiniGame",RpcTarget.All, ChooseMiniGame());
            }
        }
        #endregion
        private void GameStartReset()
        {
            currentMiniGameScene = null;
            currentMiniGameSceneIndex = -1;
            nextMiniGameScene = null;
            nextMiniGameSceneIndex = -1;
            GameInProgress = false;

            numberOfGamesLeft = GameSettings.Instance.RoundsToPlay;
            playersInRoom = PhotonNetwork.CurrentRoom.PlayerCount;
            ResetOnRound();
        }
        private void ResetOnRound()
        {
            localPlayerReady = false;
            playersReady = 0;
        }
        /*
         * Called when first mini game of round starts
         */
        [PunRPC]
        private void RPC_StartMiniGamesRounds()
        {
            GameStartReset();
            OnLoadingMiniGameAction?.Invoke();
            GameInProgress = true;
            if(PhotonNetwork.IsMasterClient)
                photonView.RPC("RPC_LoadMiniGame",RpcTarget.All, ChooseMiniGame());
        }
        [PunRPC]
        private void RPC_LoadMiniGame(int nextSceneIndex)
        {
            nextMiniGameSceneIndex = nextSceneIndex;
            
            nextMiniGameScene = SceneManager.GetSceneByBuildIndex(nextMiniGameSceneIndex);
            /*
             * StartCoroutine where we will wait for:
             *  Unloading current game if any exist
             *  Then load next
             */
            StartCoroutine(nameof(LoadMiniGame));
            PopUpMessageController.Instance.InfoPopUp("Loading minigame: " + nextMiniGameScene.Value.name);
        }
        /*
         *
         * Is only on MASTER
         */
        [PunRPC]
        private void RPC_IsPlayerReady(bool playerReady)
        {
            if (playerReady)
                playersReady++;
            else
                playersReady--;

            if (allPlayersReady)
            {
                photonView.RPC("RPC_StartMiniGame", RpcTarget.All);
            }
        }
        /*
         *
         * Called after all players are ready
         */
        [PunRPC]
        private void RPC_StartMiniGame()
        {
            OnAllPlayersReadyAction?.Invoke();
            WatingForReady = false;
        }
        [PunRPC]
        private void RPC_AllRoundsFinished()
        {
            StartCoroutine(nameof(AllRoundsFinished));
        }
        [PunRPC]
        private void RPC_AllLeave()
        {
            StartCoroutine(nameof(CancelGame));
        }
    }
}
