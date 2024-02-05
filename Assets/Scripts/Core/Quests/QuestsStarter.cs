using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StartingOneQuest
{
    public float TimeToWaitBeforeThisQuest;
    public List<OneQuestDescription> QuestsDescription;
}

public class QuestsStarter : MonoBehaviour
{
    [SerializeField] private List<StartingOneQuest> Quests = new();

    private int IndexOfNextQuestToBeStarted = 0;
    private int CounterCurrentValue = 0;
    private int NeededValueForTheTimer;
    private bool IsTimerRunning = true;

    private void Awake()
    {
        NeededValueForTheTimer = (int)(Quests[CounterCurrentValue].TimeToWaitBeforeThisQuest / Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        if (IsTimerRunning)
        {
            CounterCurrentValue++;
            if(CounterCurrentValue >= NeededValueForTheTimer)
            {
                Quests[IndexOfNextQuestToBeStarted].QuestsDescription.ForEach(questDescription => questDescription.StartQuest());
                IndexOfNextQuestToBeStarted++;
                if(IndexOfNextQuestToBeStarted >= Quests.Count)
                {
                    IsTimerRunning = false;
                    IndexOfNextQuestToBeStarted = 0;
                }
                else
                {
                    NeededValueForTheTimer = (int)(Quests[IndexOfNextQuestToBeStarted].TimeToWaitBeforeThisQuest / Time.fixedDeltaTime);
                }
                CounterCurrentValue = 0;
            }
        }
    }
}