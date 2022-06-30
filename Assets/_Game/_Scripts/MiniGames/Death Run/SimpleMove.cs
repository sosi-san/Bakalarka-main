using UnityEngine;

namespace Woska
{
    public class SimpleMove : MonoBehaviour
    {
        #region Public Fields
        
        private bool _gameStarted = false;
        [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
        [SerializeField] private Vector3 direction;
        #endregion

        #region Unity Method
        private void OnEnable()
        {
            MiniGameManager.OnMiniGameStartAction += () => _gameStarted = (true);
            MiniGameManager.MiniGameStopAction += () => _gameStarted = (false);
        }

        private void OnDisable()
        {
            MiniGameManager.OnMiniGameStartAction -= () => _gameStarted = (true);
            MiniGameManager.MiniGameStopAction -= () => _gameStarted = (false);
        }
        private void Awake()
        {
            
        }
        private void Start()
        {
            
        }
        private void Update()
        {
            
        }
        private void FixedUpdate()
        {
            if(!_gameStarted) 
                return;
            transform.position += direction * maxSpeed * Time.deltaTime;
        }

        #endregion

        #region Public Methods
        #endregion

        #region Private Methods
        
        #endregion
    }
}
