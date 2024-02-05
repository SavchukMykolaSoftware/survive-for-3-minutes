using UnityEngine;
using System;

public class ChangingFloatUniformly : MonoBehaviour
{
    private int QuantityOfIterations;
    private float DeltaValueAtOneIteration;
    protected float FinishValue;
    protected Action OnFinishedChanging;
    public float Value { get; protected set; }
    public bool IsRunningNow { get; protected set; } = false;

    protected void ChangeValue(float startValue, float finishValue, float timeInSeconds, Action onFinishedChanging = null)
    {
        if (IsRunningNow == false)
        {
            OnFinishedChanging = onFinishedChanging;
            QuantityOfIterations = (int)(timeInSeconds / Time.fixedDeltaTime);
            DeltaValueAtOneIteration = (finishValue - startValue) / QuantityOfIterations;
            Value = startValue;
            FinishValue = finishValue;
            IsRunningNow = true;
        }
    }

    protected void FixedUpdate()
    {
        if (IsRunningNow)
        {
            Value += DeltaValueAtOneIteration;
            if ((DeltaValueAtOneIteration > 0 && Value >= FinishValue) || (DeltaValueAtOneIteration < 0 && Value <= FinishValue))
            {
                IsRunningNow = false;
                Value = FinishValue;
                OnFinishedChanging?.Invoke();
            }
        }
    }
}