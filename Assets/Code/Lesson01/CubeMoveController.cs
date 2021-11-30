using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;


public sealed class CubeMoveController : MonoBehaviour
{
    [SerializeField] private Transform _cube;
    [SerializeField] private List<Transform> _checkPoints = new List<Transform>();
    [SerializeField] private float _duration = 3.0f;

    private async void Start()
    {
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        var task1 = MoveAsync(cancellationTokenSource.Token, _duration, 0);
        //cancellationTokenSource.Cancel();

    }

    public async Task<int> MoveAsync(CancellationToken cancellationToken, float duration, int startCheckPoint = 0)
    {
        while (true)
        {
            float spenTime = 0.0f;
            float t = 0.0f;
            while (t < 1.0f)
            {
                cancellationToken.ThrowIfCancellationRequested();

                t = spenTime / duration;
                _cube.position = Vector3.Lerp(_checkPoints[startCheckPoint].position, _checkPoints[startCheckPoint + 1].position, t);
                //await Task.Delay(10);
                await Task.Run(() => PrintSomethingAsyncYield());
                await Task.Yield();
                spenTime += Time.deltaTime;
            }
            if (startCheckPoint == _checkPoints.Count - 1)
            {
                startCheckPoint = 0;
                break;
            }
            else
            {
                startCheckPoint++;
            }
        }

        return startCheckPoint;
    }

    public async Task PrintSomethingAsyncYield()
    {
        Debug.Log("Print Some time");
        await Task.Yield();
    }
}
