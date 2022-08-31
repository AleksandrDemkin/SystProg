using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace DefaultNamespace
{
    public class TwoTasks : MonoBehaviour
    {
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private Task _task1;
        private Task _task2;
        private int _maxFrames = 60;

        async void Start()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;

            await Task.WhenAll(Task1(1, _cancellationToken), Task2(_maxFrames, _cancellationToken));

            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }

        async Task Task1(int seconds, CancellationToken token)
        {
            await Task.Delay(1000 * seconds, token);
            Debug.Log($"{nameof(Task1)} is completed in {seconds} second");
        }

        async Task Task2(int frame, CancellationToken token)
        {
            if (frame == _maxFrames)
            {
                if (_cancellationToken.IsCancellationRequested)
                {
                    return;
                }

                await Task.Yield();
                Debug.Log($"{nameof(Task2)} is completed on the frame {frame}");
            }
        }
    }
}