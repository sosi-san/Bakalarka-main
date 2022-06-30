using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using Random = System.Random;

namespace Woska
{
    public class PlatformController : MonoBehaviour
    {
        #region Public Fields
        public PressurePlate LeftPlate;
        public PressurePlate CenterPlate;
        public PressurePlate RightPlate;

        private GameObject helpBlock;

        public Transform leftSpawnPoint;
        public Transform rightSpawnPoint;

        public List<PlatformController> OtherControllers;
        public List<Transform> otherLeftSpawnPoints;
        public List<Transform> otherRightSpawnPoints;
        #endregion

        #region Unity Method

        private void Awake()
        {
            OtherControllers = FindObjectsOfType<PlatformController>().ToList();
            OtherControllers.Remove(this);
        }
        private void OnEnable()
        {
            LeftPlate.OnActivatedAction += LeftActivated;
            RightPlate.OnActivatedAction += RightActivated;
            CenterPlate.OnActivatedAction += CenterActivated;
        }

        private void OnDisable()
        {
            LeftPlate.OnActivatedAction -= LeftActivated;
            RightPlate.OnActivatedAction -= RightActivated;
            CenterPlate.OnActivatedAction -= CenterActivated;
        }

        private void Start()
        {
            UpdateControllers();
        }
        private void Update()
        {
            
        }

        private void UpdateControllers()
        {
            OtherControllers.RemoveAll(item => item.gameObject.activeSelf == false);
            
            otherLeftSpawnPoints.Clear();
            otherRightSpawnPoints.Clear();
            foreach (var otherController in OtherControllers)
            {
                otherLeftSpawnPoints.Add(otherController.leftSpawnPoint);
                otherRightSpawnPoints.Add(otherController.rightSpawnPoint);
            }
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void LeftActivated()
        {
            UpdateControllers();
            if(OtherControllers.Count == 0)
                return;
            PhotonNetwork.Instantiate("ChainWheel", otherLeftSpawnPoints.RandomItem().position, Quaternion.identity).TryGetComponent(out ChainWheel chainWheel);
            chainWheel.Init(1);
        }
        private void RightActivated()
        {
            UpdateControllers();
            if(OtherControllers.Count == 0)
                return;
            PhotonNetwork.Instantiate("ChainWheel", otherRightSpawnPoints.RandomItem().position, Quaternion.identity).TryGetComponent(out ChainWheel chainWheel);
            chainWheel.Init(-1);
        }
        private void CenterActivated()
        {
            UpdateControllers();
            if(OtherControllers.Count == 0)
                return;
            foreach (var spawnPoint in otherLeftSpawnPoints)
            {
                PhotonNetwork.Instantiate("ChainWheel", spawnPoint.position, Quaternion.identity).TryGetComponent(out ChainWheel chainWheel);
                chainWheel.Init(1);
            }
            foreach (var spawnPoint in otherRightSpawnPoints)
            {
                PhotonNetwork.Instantiate("ChainWheel", spawnPoint.position, Quaternion.identity).TryGetComponent(out ChainWheel chainWheel);
                chainWheel.Init(-1);
            }
        }

        #endregion
    }
}
