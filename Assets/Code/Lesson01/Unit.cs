using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Unit : MonoBehaviour
    {
        #region Fields

        [SerializeField] private int _health = 80;
        [SerializeField] private Button _buttonHealing;
        [SerializeField] private TMP_Text _textHealing;
        [Tooltip("The number of added lives.")]
        [SerializeField] private int _healthHealing = 5;
        [Tooltip("Checking the treatment condition.")]
        [SerializeField] private bool _isHealing = false;

        private int _maxHealth = 100;
        private float _timeHealing = 3.0f;
        private float _defaultTimeHealing = 3.0f;

        #endregion


        #region UnityMethods

        private void Start()
        {
            Reset();
            _buttonHealing.onClick.AddListener(RecieveHealing);
            SetTextHealthUI(_health);
        }

        #endregion


        #region ClassLifeCycles

        ~Unit()
        {
            _buttonHealing.onClick.RemoveListener(RecieveHealing);
        }

        #endregion


        #region Methods

        private void RecieveHealing()
        {
            if (_isHealing)
            {
                Debug.Log($"Лечение уже производится - {_isHealing}");
                return;
            }
            StartCoroutine(HealingCourutine(_healthHealing));
        }

        private IEnumerator HealingCourutine(int healthValue)
        {
            _isHealing = true;
            while (_health < _maxHealth && _timeHealing >= 0.0f)
            {
                _health += healthValue;
                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
                SetTextHealthUI(_health);
                _timeHealing -= 0.5f;
                yield return new WaitForSeconds(0.5f);
            }
            Reset();
            yield return new WaitForEndOfFrame();
        }

        private void Reset()
        {
            _isHealing = false;
            _timeHealing = _defaultTimeHealing;
        }

        private void SetTextHealthUI(int health)
        {
            _textHealing.text = health.ToString();
        }

        #endregion
    }
}
