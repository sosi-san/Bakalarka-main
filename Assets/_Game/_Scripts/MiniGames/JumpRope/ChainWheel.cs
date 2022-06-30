using Photon.Pun;
using UnityEngine;

namespace Woska
{
    public class ChainWheel : MonoBehaviourPun
    {
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float wheelSpeed;
        [SerializeField] private AnimationCurve _animationCurve;

        private Vector3 startPosition;
        private Vector3 targetPosition;

        private float timePosition = 0f;

        private Transform wheel;
        private int _direction;
        #region Public Fields
        #endregion

        #region Unity Method

        private void Awake()
        {
            wheel = transform.GetChild(0);
        }

        public void Init(int direction)
        {
            photonView.RPC("RPC_Init", RpcTarget.All, direction);
        }
        private void Update()
        {
            float angle = _direction*rotationSpeed * (180f / Mathf.PI) / transform.lossyScale.x*0.5f;
            wheel.rotation = Quaternion.Euler(Vector3.forward * angle) * wheel.rotation;

            if (targetPosition != transform.position)
            {
                timePosition += Time.deltaTime*wheelSpeed;
                transform.position =
                    Vector3.Lerp(startPosition, targetPosition, _animationCurve.Evaluate(timePosition));
            }
            else
            {
                if(!PhotonNetwork.IsMasterClient)
                    return;
                PhotonNetwork.Destroy(gameObject);
            }
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        [PunRPC]
        private void RPC_Init(int direction)
        {
            _direction = direction;
            startPosition = transform.position;
            targetPosition = startPosition + Vector3.right * 8 * _direction;
        }
        #endregion
    }
}
