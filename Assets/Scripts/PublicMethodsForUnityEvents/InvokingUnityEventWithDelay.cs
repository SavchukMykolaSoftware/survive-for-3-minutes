using UnityEngine;
using UnityEngine.Events;

public class InvokingUnityEventWithDelay : MonoBehaviour
{
    [SerializeField] private UnityEvent EventToBeInvokedWithDelay;

    private bool IsCounterRunning = false;
    private int CounterCurrentValue = 0;
    private int CounterNeededValue;

    public void InvokeEventWithDelay(float delay)
    {
        CounterNeededValue = (int)(delay / Time.fixedDeltaTime);
        IsCounterRunning = true;
    }

    private void FixedUpdate()
    {
        if(IsCounterRunning)
        {
            CounterCurrentValue++;
            if(CounterCurrentValue >= CounterNeededValue)
            {
                EventToBeInvokedWithDelay.Invoke();
                CounterCurrentValue = 0;
                IsCounterRunning = false;
            }
        }
    }
}