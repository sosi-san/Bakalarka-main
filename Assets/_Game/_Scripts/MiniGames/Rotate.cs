using System;
using UnityEngine;

namespace Woska
{
    public class Rotate : MonoBehaviour
    {
        #region Public Fields
        private Rigidbody2D _rigidbody2D;
        #endregion

        #region Unity Method

        private void Awake()
        {
            _rigidbody2D = GetComponentInParent<Rigidbody2D>();
        }
        private void Start()
        {
            
        }
        private void Update()
        {
            RotateWheel();
        }

        private void FixedUpdate()
        {
            //RotateWheel();
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void RotateWheel()
        {
            Vector3 movement = _rigidbody2D.velocity * Time.deltaTime;

            float distance = movement.magnitude;
            float angle = distance * (180f / Mathf.PI) / transform.lossyScale.x*0.5f;
            transform.localRotation = Quaternion.Euler(Mathf.Sign(-movement.x)*Vector3.forward * angle) * transform.localRotation;
        }
        #endregion
    }
}
