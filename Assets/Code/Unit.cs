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

        private int _maxHealth = 100;
        private float _timeHealing = 3.0f;

        #endregion


        #region UnityMethods

        private void Start()
        {
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
            StartCoroutine(HealingCourutine(_healthHealing));
        }

        private IEnumerator HealingCourutine(int healthValue)
        {
            while (_health < _maxHealth && _timeHealing >= 0.0f)
            {
                _health += healthValue;
                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }
                SetTextHealthUI(_health);
                _timeHealing -= Time.deltaTime;
                yield return new WaitForSeconds(0.5f);
            }
            yield break;
        }

        private void SetTextHealthUI(int health)
        {
            _textHealing.text = health.ToString();
        }

        #endregion
    }
}
