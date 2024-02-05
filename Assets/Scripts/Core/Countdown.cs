using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class EventInSpecificMoment
{
    public float TimeMoment;
    public UnityEvent OnReachingThisTimeMoment;
    public bool WasThisEventInvoked { get; set; }
}

public class Countdown : MonoBehaviour
{
    [SerializeField] private float TimerStartValueInSeconds;
    [SerializeField] private TextMeshProUGUI TextWithTimer;
    [SerializeField] private List<EventInSpecificMoment> EventsInSpecificMoments = new();

    private int TimerCurrentValueInFixedDeltaTime;

    private void Awake()
    {
        TimerCurrentValueInFixedDeltaTime = (int)(TimerStartValueInSeconds / Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        TimerCurrentValueInFixedDeltaTime--;
        int TimerCurrentValueInSeconds = (int)(TimerCurrentValueInFixedDeltaTime * Time.fixedDeltaTime);
        TextWithTimer.text = $"{TimerCurrentValueInSeconds / 60:00}:{TimerCurrentValueInSeconds % 60:00}";
        foreach (EventInSpecificMoment eventInSpecificMoment in EventsInSpecificMoments)
        {
            if (eventInSpecificMoment.WasThisEventInvoked == false && TimerCurrentValueInSeconds <= eventInSpecificMoment.TimeMoment)
            {
                eventInSpecificMoment.OnReachingThisTimeMoment.Invoke();
                eventInSpecificMoment.WasThisEventInvoked = true;
            }
        }
    }
}