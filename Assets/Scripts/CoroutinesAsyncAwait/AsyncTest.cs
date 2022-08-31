using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class AsyncTest : MonoBehaviour
{
    [SerializeField] private Button _button;
    
    private string _message = "Message";
    private int _times = 50;
    private Task _task1;
    private Task _task2;
    private Task _task3;
    private Task _task4;
    private double _time;
    
    async void Start()
    {
        Task<string> task = await Task.WhenAny(MyTimer("1", 1), 
            MyTimer("2", 2), MyTimer("3", 3));
        Debug.Log($"$Result: {task.Result}");
    }

    #region CancellationTokenSource
    
    /*private CancellationTokenSource _cancellationTokenSource;
        
    private void Start()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        CancellationToken cancellationToken = _cancellationTokenSource.Token;

        Task task = Task1(1, cancellationToken);
        _cancellationTokenSource.Cancel();
    }
        
    async Task Task1(int seconds, CancellationToken token)
    {
        await Task.Delay(1000 * seconds, token);
    }

    private void OnDestroy()
    {
        _cancellationTokenSource.Dispose();
    }*/

    #endregion

    #region MyTimer

    //Task<string> task = await Task.WhenAny(MyTimer("1", 1), MyTimer("2", 2), MyTimer("3", 3));
    //Debug.Log($"$Result: {task.Result}");

    async Task<string> MyTimer(string name1, int seconds)
    {
        await Task.Delay(1000 * seconds);

        return name1;
    }

    #endregion

    #region FactorialAsync
    // async void Start()
    // {
    //     CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
    //     CancellationToken cancelToken = cancelTokenSource.Token;
    //     Task task = new Task(() => FactorialAsync(cancelToken, 5));
    //     task.Start();
    //     cancelTokenSource.Cancel();
    //     cancelTokenSource.Dispose();
    // }
    async Task<long> FactorialAsync(CancellationToken cancelToken, int x)
    {
        int result = 1;
        for (int i = 1; i < x; i++)
        {
            if (cancelToken.IsCancellationRequested)
            {
                Debug.Log("Операция прервана токеном.");
                return result;
            }
            result *= i;
            await Task.Yield();
        }
        return result;
    }
    #endregion

    #region WaitRandomTime
    // async void Start()
    // {
    //     Task<int> _task3 = WaitRandomTime();
    //     Task<int> _task4 = WaitRandomTime();
    //     var taskResult = await Task.WhenAny(_task3, _task4);
    //     Debug.Log(taskResult.Result);
    // }
    async Task<int> WaitRandomTime()
    {
        int rnd = Random.Range(100, 1000);
        await Task.Delay(rnd);
        return rnd;
    }
    #endregion

    #region StartAsync
    // async void Start()
    // {
    //     _time = await PressButtonTimer(_button);
    //     Debug.Log(_time);
    // }
    #endregion

    #region UnitTasksAsync
    //UnitTasksAsync();
    async void UnitTasksAsync()
    {
       await Task.WhenAll(Unit1Async(), Unit2Async());
        Debug.Log($"{nameof(Unit1Async)} and {nameof(Unit2Async)} are completed");
    }

    async Task Unit1Async()
    {
        Debug.Log("Unit1 starts chopping wood.");
        await Task.Delay(3000);
        Debug.Log("Unit1 finishes chopping wood.");
    }
    
    async Task Unit2Async()
    {
        Debug.Log("Unit2 starts patrolling.");
        await Task.Delay(5000);
        Debug.Log("Unit2 finishes patrolling.");
    }

    #endregion
    
    #region PrintAsync
    //PrintAsync1(_message, _times);
    async void PrintAsync1(string message, int times)
    {
        while (times > 0)
        {
            times--;
            Debug.Log($"Message: {times}");
            await Task.Yield();
        }
    }

    async void PrintAsync()
    {
        Debug.Log($"Message was printed instantly.");
        await Task.Delay(1000);
        Debug.Log($"Message was printed over 1 second.");
    }
    #endregion
}
