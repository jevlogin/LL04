using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class MyTextField : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _textObject;
        [SerializeField] private Scrollbar _scrollbar;
        private List<string> _messages = new List<string>();

        private void Start()
        {
            _scrollbar.onValueChanged.AddListener((float value) => UpdateText());
            _textObject.text = string.Empty;
        }

        private void OnDestroy()
        {
            _scrollbar.onValueChanged.RemoveAllListeners();
        }

        private void UpdateText()
        {
            string text = string.Empty;
            int index = (int)(_messages.Count * _scrollbar.value);
            for (int i = index; i < _messages.Count; i++)
            {
                text += _messages[i] + "\n";
            }
            _textObject.text = text;
        }

        public void ReceiveMessage(object message)
        {
            _messages.Add(message.ToString());
            float value = (_messages.Count - 1) * _scrollbar.value;
            _scrollbar.value = Mathf.Clamp(value, 0.0f, 1.0f);
            UpdateText();
        }
    }
}
