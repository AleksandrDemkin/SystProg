using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public delegate void CloseHandler(bool accepted);

public class SomePopup : MonoBehaviour
{
    [SerializeField] private Button _buttonAccept;
    [SerializeField] Button _buttonCancel;

    public CloseHandler OnClose;
    
    public async void ActivatePopup(CancellationToken ct)
    {
        using (CancellationTokenSource linkedCts =
            CancellationTokenSource.CreateLinkedTokenSource(ct))
        {
            CancellationToken linkedCt = linkedCts.Token;
            Task<bool> task1 = PressButtonAsync(linkedCt, _buttonAccept);
            Task<bool> task2 = PressButtonAsync(linkedCt, _buttonCancel);
            Task<bool> finishedTask = await Task.WhenAny(task1, task2);
            bool result = (finishedTask == task1 && finishedTask.Result == true);
            linkedCts.Cancel();
            OnClose?.Invoke(result);
        }
    }                                                                                                                                                                                                                                                                                                                                                                                                

    async Task<bool> PressButtonAsync(CancellationToken ct, Button button)
    {
        bool isPressed = false;
        button.onClick.AddListener(() => isPressed = true);
        while (isPressed == false)
        {
            if (ct.IsCancellationRequested)
            {
                return false;
            }
            await Task.Yield();
        }
        return true;
    }
    
    [SerializeField] private GameObject _prefabPopup;
    public void TryBuyItem()
    {
        GameObject newPopup = Instantiate(_prefabPopup);
        SomePopup popupScript = newPopup.GetComponent<SomePopup>();
        popupScript.OnClose+=CompletePurhase;
        CancellationTokenSource cts = new CancellationTokenSource();
        CancellationToken ct = cts.Token;
        popupScript.ActivatePopup(ct);
    }
    
    private void CompletePurhase(bool completed)
    {
        if (completed) Debug.Log("Покупка совершена!");
        else Debug.Log("Покупка отменена!");
    }
}