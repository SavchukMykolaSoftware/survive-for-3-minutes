using UnityEngine;
using System;
using System.Collections.Generic;

public class MaterialsWhichChangeTransparencyUniformly : ChangingFloatUniformly
{
    [SerializeField] private List<Material> MaterialsWhichTransparencyIsChanged;
    [SerializeField] private float StartTransparency;

    private void SetTransparency(float newTransparency)
    {
        foreach (Material oneMaterial in MaterialsWhichTransparencyIsChanged)
        {
            Color CurrentColor = oneMaterial.color;
            CurrentColor.a = newTransparency;
            oneMaterial.color = CurrentColor;
        }
    }

    private new void FixedUpdate()
    {
        if (IsRunningNow)
        {
            base.FixedUpdate();
            SetTransparency(Value);
        }
    }

    public void StartChangingTransparency(float startValue, float finishValue, float timeInSeconds, Action onFinishedChanging = null)
    {
        onFinishedChanging += () => SetTransparency(finishValue);
        ChangeValue(startValue, finishValue, timeInSeconds, onFinishedChanging);
    }

    private void Awake()
    {
        SetTransparency(StartTransparency);
    }
}