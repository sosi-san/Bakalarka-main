using System.Collections;
using System.Collections.Generic;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace Woska
{
    public class JumpRopeController : MiniGameManager
    {
        #region Public Fields

        [SerializeField] private PlatformController[] platformControllers;
        #endregion

        #region Unity Method

        protected override void SetUpGame()
        {
            base.SetUpGame();

            for(int i = PlayerNumbering.SortedPlayers.Length; i < platformControllers.Length; i++)
            {
                platformControllers[i].gameObject.SetActive(false);
            }

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
            yield return null;
        }

        #endregion
    }
}
