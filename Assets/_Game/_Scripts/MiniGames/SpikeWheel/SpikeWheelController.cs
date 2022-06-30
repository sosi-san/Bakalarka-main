using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;

namespace Woska
{
    public class SpikeWheelController : MiniGameManager
    {
        #region Public Fields

        [SerializeField] private GameObject wheelPrefab;

        #endregion

        #region Unity Method

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        protected override void GameLoop()
        {
            StartCoroutine(nameof(Logic));
            StartCoroutine(nameof(GameTime));
        }
        

        protected override void CleanUp()
        {
            MiniGameStopAction?.Invoke();
            StopCoroutine(nameof(GameTime));
            
            if (!PhotonNetwork.IsMasterClient)
                return;
            StopCoroutine(nameof(Logic));

            foreach (var VARIABLE in allWheels)
            {
                if (VARIABLE != null)
                    PhotonNetwork.Destroy(VARIABLE);
            }
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
            if (PhotonNetwork.IsMasterClient)
            {
                WheelController a = null;
                while (gameTime >= 0)
                {
                    switch (Random.Range(0, 6))
                    {
                        case 0:
                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.FULL, 1, WheelSize.SMALL, WheelSpeed.FAST);
                            allWheels.Add(a.gameObject);

                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.FULL, -1, WheelSize.SMALL, WheelSpeed.FAST);
                            allWheels.Add(a.gameObject);

                            yield return Helpers.GetWait(3f);
                            break;
                        case 1:
                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.FULL, 1, WheelSize.SMALL, WheelSpeed.FAST);
                            allWheels.Add(a.gameObject);

                            yield return Helpers.GetWait(3f);
                            break;
                        case 2:
                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.FULL, -1, WheelSize.SMALL, WheelSpeed.FAST);
                            allWheels.Add(a.gameObject);

                            yield return Helpers.GetWait(3f);
                            break;

                        case 3:
                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.QUARTER, 1, WheelSize.NORMAL, WheelSpeed.NORMAL);
                            allWheels.Add(a.gameObject);

                            yield return Helpers.GetWait(3f);
                            break;
                        case 4:
                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.QUARTER, -1, WheelSize.NORMAL, WheelSpeed.NORMAL);
                            allWheels.Add(a.gameObject);

                            yield return Helpers.GetWait(3f);
                            break;
                        case 5:
                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.HALF, 1, WheelSize.BIG, WheelSpeed.SLOW);
                            allWheels.Add(a.gameObject);
                            yield return Helpers.GetWait(3f);
                            break;
                        case 6:
                            a = PhotonNetwork.Instantiate(wheelPrefab.name, Vector3.up * 42, Quaternion.identity)
                                .GetComponent<WheelController>();
                            a.Init(WheelType.HALF, -1, WheelSize.BIG, WheelSpeed.SLOW);
                            allWheels.Add(a.gameObject);
                            yield return Helpers.GetWait(3f);
                            break;
                    }
                }
            }

            yield return null;
        }

        #endregion
    }
}