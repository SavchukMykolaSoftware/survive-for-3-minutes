using UnityEngine;
using UnityEngine.Events;

public class MovingFence : OneQuestDescription
{
    [SerializeField] private Rigidbody FirstFenceSection;
    [SerializeField] private Rigidbody SecondFenceSection;
    [SerializeField] private Axis MovementAxis;
    [SerializeField, Range(0, 1)] private float PercentOfFreeGap = 0.25f;
    [SerializeField] private float Speed;
    [SerializeField] private int QuantityOfIterations = 3;
    [SerializeField] private UnityEvent BeforeStarting;
    [SerializeField] private UnityEvent AfterFinishing;

    private Vector3 MovementAxisAsVector3;
    private Vector3 VelocityVector;
    private Vector3 FirstFenceSectionStartingPosition;
    private Vector3 SecondFenceSectionStartingPosition;
    private float StartDistance;
    private bool IsFirstIterationRunningNow = false;
    private int CurrentDirectionAsSign = 1;
    private float FirstFenceSectionPositionAtStartOfThisIteration;
    private float DistanceWhichShouldBeOvercomeInThisIteration;
    private bool IsUsualIterationRunningNow = false;
    private int NumberOfCurrentIteration = 0;
    private bool IsLastIterationRunningNow = false;

    private float GetCurrentPositionAlongSelectedAxis(Component component)
    {
        return component.transform.position[(int)MovementAxis];
    }

    private float GetCurrentDistanceBetweenFenceSections()
    {
        return Mathf.Abs(GetCurrentPositionAlongSelectedAxis(FirstFenceSection) - GetCurrentPositionAlongSelectedAxis(SecondFenceSection));
    }

    private void Awake()
    {
        MovementAxisAsVector3 = Vector3.zero;
        MovementAxisAsVector3[(int)MovementAxis] = 1;
        VelocityVector = Speed * MovementAxisAsVector3;
        FirstFenceSectionStartingPosition = FirstFenceSection.transform.position;
        SecondFenceSectionStartingPosition = SecondFenceSection.transform.position;
        StartDistance = GetCurrentDistanceBetweenFenceSections();
    }

    private void FirstIteration()
    {
        BeforeStarting.Invoke();
        FirstFenceSection.transform.position = FirstFenceSectionStartingPosition;
        SecondFenceSection.transform.position = SecondFenceSectionStartingPosition;
        FirstFenceSection.velocity = VelocityVector;
        SecondFenceSection.velocity = VelocityVector;
        if (GetCurrentPositionAlongSelectedAxis(FirstFenceSection) > GetCurrentPositionAlongSelectedAxis(SecondFenceSection))
        {
            FirstFenceSection.velocity *= -1;
        }
        else
        {
            SecondFenceSection.velocity *= -1;
        }
        IsFirstIterationRunningNow = true;
    }

    private void LastIteration()
    {
        FirstFenceSection.velocity = VelocityVector;
        SecondFenceSection.velocity = VelocityVector;
        if (GetCurrentPositionAlongSelectedAxis(FirstFenceSection) < GetCurrentPositionAlongSelectedAxis(SecondFenceSection))
        {
            FirstFenceSection.velocity *= -1;
        }
        else
        {
            SecondFenceSection.velocity *= -1;
        }
        IsLastIterationRunningNow = true;
    }

    private void OneUsualIteration(int directionAsSign)
    {
        NumberOfCurrentIteration++;
        FirstFenceSectionPositionAtStartOfThisIteration = GetCurrentPositionAlongSelectedAxis(FirstFenceSection);
        CurrentDirectionAsSign = directionAsSign;
        FirstFenceSection.velocity = VelocityVector * CurrentDirectionAsSign;
        SecondFenceSection.velocity = VelocityVector * CurrentDirectionAsSign;
        DistanceWhichShouldBeOvercomeInThisIteration = StartDistance * (1 - PercentOfFreeGap) / 2;
        IsUsualIterationRunningNow = true;
    }

    private void OneUsualIteration()
    {
        OneUsualIteration(new System.Random().Next(0, 2) == 0 ? 1 : -1);
    }

    private void FixedUpdate()
    {
        if(IsFirstIterationRunningNow && GetCurrentDistanceBetweenFenceSections() <= PercentOfFreeGap * StartDistance)
        {
            IsFirstIterationRunningNow = false;
            OneUsualIteration();
        }
        if(IsUsualIterationRunningNow && Mathf.Abs(GetCurrentPositionAlongSelectedAxis(FirstFenceSection) - FirstFenceSectionPositionAtStartOfThisIteration) > DistanceWhichShouldBeOvercomeInThisIteration)
        {
            IsUsualIterationRunningNow = false;
            if (NumberOfCurrentIteration % 2 == 1)
            {
                OneUsualIteration(-CurrentDirectionAsSign);
            }
            else
            {
                if (QuantityOfIterations * 2 == NumberOfCurrentIteration)
                {
                    LastIteration();
                }
                else
                {
                    OneUsualIteration();
                }
            }
        }
        if(IsLastIterationRunningNow && GetCurrentDistanceBetweenFenceSections() >= StartDistance)
        {
            FirstFenceSection.velocity = Vector3.zero;
            SecondFenceSection.velocity = Vector3.zero;
            IsLastIterationRunningNow = false;
            NumberOfCurrentIteration = 0;
            AfterFinishing.Invoke();
        }
    }

    public override void StartQuest()
    {
        FirstIteration();
    }
}