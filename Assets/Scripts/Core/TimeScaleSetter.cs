using UnityEngine;

public class TimeScaleSetter : MonoBehaviour
{
    [SerializeField] private float TimeScaleToBeSet = 1;

    private void Awake()
    {
        SetTimeScale(TimeScaleToBeSet);
    }

    public void SetTimeScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }
}