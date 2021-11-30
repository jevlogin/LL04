using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Task03 : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        #region Fields

        [SerializeField] private Button _buttonHealing;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isPointerDownButton = false;
        private bool _isPointerUpButton = false;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _buttonHealing.onClick.AddListener(() => TryBuyItem());
            //_buttonHealing.OnPointerDown.AddListener(() => TryBuyItem());
        }

        #endregion


        #region lassLifeCycles

        ~Task03()
        {
            _buttonHealing.onClick.RemoveAllListeners();
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        #endregion


        #region Methods

        public async void TryBuyItem()
        {
            Debug.Log($"TryBuyItem");

            _cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = _cancellationTokenSource.Token;

            await WhatTaskFasterAsync(cancellationToken, Task01(cancellationToken), Task02(cancellationToken));
        }


        public static async Task<bool> WhatTaskFasterAsync(CancellationToken token, Task task1, Task task2)
        {
            Debug.Log($"WhatTaskFasterAsync");

            token.ThrowIfCancellationRequested();

            var task = Task.WhenAny(task1, task2);
            await task;

            if (task.IsCompleted)
            {
                if (task1.IsCompleted)
                {
                    Debug.Log($"task1.IsCompleted");

                    task2?.Dispose();
                }
                else
                {
                    Debug.Log($"task2.IsCompleted");

                    task1?.Dispose();
                }
            }

            return true;
        }

        private async Task<bool> Task01(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            while (!_isPointerDownButton)
            {
                await Task.Yield();
            }

            return _isPointerDownButton;
        }

        private async Task<bool> Task02(CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            while (!_isPointerUpButton)
            {
                await Task.Yield();
            }

            return _isPointerUpButton;
        }

        #endregion


        #region IPointerDownHandler

        public void OnPointerDown(PointerEventData eventData)
        {
            Debug.Log("OnPointerDown");
            _isPointerDownButton = true;
        }

        #endregion


        #region IPointerUpHandler

        public void OnPointerUp(PointerEventData eventData)
        {
            _isPointerUpButton = true;
        }

        #endregion
    }
}
