using System;
using Photon.Pun;
using UnityEngine;

namespace Woska
{
    public class WheelController : MonoBehaviourPun
    {
        #region Public Fields
        private Rigidbody2D _rigidbody2D;
        public Vector2 Velocity => _rigidbody2D.velocity;

        private WheelSpeed _wheelSpeed;
        private WheelSize _wheelSize;
        private int _direction;
        private WheelType _wheelType;
        #endregion

        #region Unity Method

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void Init(WheelType type,int direction, WheelSize size, WheelSpeed speed)
        {
            photonView.RPC("RPC_InitWheel",RpcTarget.All, type, direction, size, speed);
        }
        private void Start()
        {
         
        }
        private void Update()
        {
            
        }

        private void FixedUpdate()
        {
            var previousVelocity = Velocity;
            var velocityChange = Vector2.zero;
        
            velocityChange.x = (_direction * (int)_wheelSpeed - previousVelocity.x);
            
            _rigidbody2D.AddForce(velocityChange, ForceMode2D.Impulse);
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods

        private void OnTriggerEnter2D(Collider2D col)
        {
            if(!PhotonNetwork.IsMasterClient) 
                return;
            if (col.gameObject.IsOnLayer(LayerMask.GetMask("WheelKillZone")))
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        #endregion

        [PunRPC]
        private void RPC_InitWheel(WheelType type,int direction, WheelSize size, WheelSpeed speed)
        {
            _direction = direction;
            _wheelSize = size;
            _wheelSpeed = speed;
            _wheelType = type;
            transform.localScale = Vector3.one*(int)size;
            transform.position = new Vector3(0,-3) + -direction * Vector3.right*15;

            if (_wheelType == WheelType.FULL)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else if (_wheelType == WheelType.QUARTER)
            {
                transform.GetChild(1).gameObject.SetActive(true);
            }
            else if (_wheelType == WheelType.HALF)
            {
                transform.GetChild(2).gameObject.SetActive(true);
            }
        }
    }
    public enum WheelSize
    {
        SMALL = 1,
        NORMAL = 5,
        BIG = 10,
        
    }
    public enum WheelSpeed
    {
        SLOW = 1,
        NORMAL = 5,
        FAST = 10,
        
    }
    public enum WheelType
    {
        FULL,
        QUARTER,
        HALF
    }
}
