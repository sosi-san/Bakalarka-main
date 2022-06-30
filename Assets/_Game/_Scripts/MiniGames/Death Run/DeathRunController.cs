using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Woska
{
    public class DeathRunController : MiniGameManager
    {
        #region Public Fields

        [SerializeField] private Transform[] obstacleSpawnPositionsBottom;
        [SerializeField] private Transform[] obstacleSpawnPositionsTop;
        [SerializeField] private Obsticle _obsticle;
        [SerializeField] private Obsticle _obsticleDuck;
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
        #endregion

         #region Private Methods
        protected override void GameLoop()
        {
            StartCoroutine(nameof(Logic));
            StartCoroutine(nameof(GameTime));
        }
        private IEnumerator GameTime()
        {
            var timeLeft = gameTime;
            OnGameTickAction?.Invoke(timeLeft);

            while (timeLeft > -1)
            {
                yield return Helpers.GetWait(1f);
                OnGameTickAction?.Invoke(timeLeft);
                timeLeft--;
            }

            StartCoroutine(nameof(GameEnd));
        }
        
        private IEnumerator Logic()
        {
            if (!PhotonNetwork.IsMasterClient) 
                yield break;
            while (gameTime >= -0)
            {
                var bottom = Helpers.RandomSign();
                for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
                {
                    if(bottom == 1)
                        PhotonNetwork.Instantiate(_obsticle.name, obstacleSpawnPositionsBottom[i].position, Quaternion.identity);
                    else
                        PhotonNetwork.Instantiate(_obsticleDuck.name, obstacleSpawnPositionsTop[i].position,  Quaternion.Euler(0,0,180f));
                }
                var randomTime = Random.Range(0.5f, 1f);
                yield return Helpers.GetWait(randomTime);
            }
            yield return null;
        }
        #endregion
    }
}
