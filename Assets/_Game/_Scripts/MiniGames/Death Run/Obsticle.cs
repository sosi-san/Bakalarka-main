using System;
using Photon.Pun;
using UnityEngine;

namespace Woska
{
    public class Obsticle : MonoBehaviourPun
    {
        [SerializeField] private float speed;
        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            transform.position += speed * Vector3.left * Time.deltaTime;
        }
        private void OnTriggerEnter2D(Collider2D col)
        {
            if(col.CompareTag("GravityOn"))
                _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
        }

        private void OnTriggerExit2D(Collider2D other)
        { 
            if(other.CompareTag("KillZone"))
                Destroy(gameObject);
        }
    }
}