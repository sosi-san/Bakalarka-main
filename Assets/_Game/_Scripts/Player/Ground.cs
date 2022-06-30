using System;
using UnityEngine;

namespace Woska
{
    public class Ground : MonoBehaviour
    {
        #region Public Fields
        [Header("Ground settings")] 
        [SerializeField] private LayerMask groundLayer;
        [SerializeField, Range(0f,1f)] private float minGroundNormal = 0.99f;
        public bool OnGround => onGround;
        private bool onGround;

        #endregion

        #region Unity Method
        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        private void EvaluateCollision(Collision2D collision)
        {
            if (!collision.gameObject.IsOnLayer(groundLayer))
                return;

            foreach (var contactPoint in collision.contacts)
            {
                Vector2 normal = contactPoint.normal;
                onGround |= normal.y >= minGroundNormal;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            EvaluateCollision(collision);
        }
        private void OnCollisionStay2D(Collision2D collision)
        {
            EvaluateCollision(collision);
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            onGround = false;
        }
        
        #endregion
    }
}
