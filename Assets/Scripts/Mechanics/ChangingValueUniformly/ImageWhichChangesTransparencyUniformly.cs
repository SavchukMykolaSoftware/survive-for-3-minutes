using UnityEngine;
using UnityEngine.UI;
using System;

public class ImageWhichChangesTransparencyUniformly : ChangingFloatUniformly
{
    [SerializeField] private Image ImageWhichTransparencyIsChanged;

    private void SetTransparency(float newTransparency)
    {
        Color CurrentColor = ImageWhichTransparencyIsChanged.color;
        CurrentColor.a = newTransparency;
        ImageWhichTransparencyIsChanged.color = CurrentColor;
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
        IsRunningNow = false;
        ChangeValue(startValue, finishValue, timeInSeconds, onFinishedChanging);
    }

    public void ShowGradually(float timeOfShowing, System.Action afterShowing) => StartChangingTransparency(0, 1, timeOfShowing, afterShowing);
    public void ShowGradually(float timeOfShowing) => ShowGradually(timeOfShowing, null);

    public void HideGradually(float timeOfHiding, System.Action afterHiding) => StartChangingTransparency(ImageWhichTransparencyIsChanged.color.a, 0, timeOfHiding, afterHiding);
    public void HideGradually(float timeOfHiding) => HideGradually(timeOfHiding, null);
}