using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Woska
{
    public class PlayerScore : MonoBehaviour
    {
        #region Public Fields
        private TextMeshProUGUI _text;
        private Image _scoreImage;
        private TextMeshProUGUI _scoreText;
        [SerializeField] private int forPlayerID;
        private Player[] _players;
        #endregion

        #region Unity Method
        private void OnEnable()
        {
            _players = PlayerNumbering.SortedPlayers;
            UpdateSlot();
        }

        private void OnDisable()
        {
            
        }
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _scoreImage = GetComponentInChildren<Image>();
            _scoreText = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
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
        private void UpdateSlot()
        {
            if(_players == null) 
                return;
            if (forPlayerID < _players.Length)
            {
                _text.text = _players[forPlayerID].NickName;
                var score = _players[forPlayerID].GetScore();
                Debug.Log(score);
                var maxScorePossible = GameSettings.Instance.RoundsToPlay * GameSettings.Instance.ScoreForWin +0f;
                Debug.Log(maxScorePossible);
                _scoreImage.fillAmount = Mathf.Clamp(score/maxScorePossible, 0f, 1f);
                _scoreText.text = _players[forPlayerID].GetScore().ToString();
            }
            else
            {
                _text.text = "EMPTY";
                
                _scoreImage.fillAmount = 0.00f;
                _scoreText.text = "0";
            }
        }
        #endregion
    }
}
