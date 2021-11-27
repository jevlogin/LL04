using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public sealed class Task02 : MonoBehaviour
    {
        #region Fields

        private CancellationTokenSource _cts;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _cts = new CancellationTokenSource();
            Tasks01(_cts.Token);
            Tasks02(_cts.Token);
        }

        #endregion


        #region ClassLifeCycle

        ~Task02()
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        #endregion


        #region Methods

        private async Task Tasks01(CancellationToken cancellationToken)
        {
            await Task.Delay(1000);
            Debug.Log("End Task");
            cancellationToken.ThrowIfCancellationRequested();
        }

        private async Task Tasks02(CancellationToken cancellationToken)
        {
            Debug.Log($"Do - {Time.frameCount}");
            for (int i = 0; i < 100; i++)
            {
                await Task.Yield();
            }
            Debug.Log($"After - {Time.frameCount}");
            cancellationToken.ThrowIfCancellationRequested();
        }

        #endregion
    }
}
