using System.Collections.Generic;
using System;
using UnityEngine;

public class SpawningObstaclesAsSquare : OneQuestDescription
{
    [SerializeField] private GameObject ObstaclePrefab;
    [SerializeField] private AudioPauser AudioPauser;
    [SerializeField] private int SquareSide = 10;
    [SerializeField] private float IntervalBetweenNeighboringObjects = 5.5f;
    [SerializeField] private Vector3 MinimalPossiblePositionOfLeftTopCorner;
    [SerializeField] private Vector3 MaximalPossiblePositionOfLeftTopCorner;
    [SerializeField] private Axis AxisPerpendicularToSquare = Axis.Y;

    public override void StartQuest()
    {
        Vector3 PositionOfLeftTopCorner = new Vector3(
            RandomNumbersGenerator.GenerateRandomFloatNumber(MinimalPossiblePositionOfLeftTopCorner.x, MaximalPossiblePositionOfLeftTopCorner.x),
            RandomNumbersGenerator.GenerateRandomFloatNumber(MinimalPossiblePositionOfLeftTopCorner.y, MaximalPossiblePositionOfLeftTopCorner.y),
            RandomNumbersGenerator.GenerateRandomFloatNumber(MinimalPossiblePositionOfLeftTopCorner.z, MaximalPossiblePositionOfLeftTopCorner.z));
        Spawn(ObstaclePrefab, SquareSide, IntervalBetweenNeighboringObjects, PositionOfLeftTopCorner, AxisPerpendicularToSquare);
    }

    private void Spawn(GameObject objectTemplate, int squareSide, float intervalBetweenNeighboringObjects, Vector3 positionOfLeftTopCorner, Axis axisPerpendicularToSquare)
    {
        Tuple<int, int> AxisParallelToSquareSideAsInt = new(int.MaxValue, int.MaxValue);
        for (int i = 0; i <= 2; i++)
        {
            if (i != (int)(axisPerpendicularToSquare))
            {
                if (AxisParallelToSquareSideAsInt.Item1 == int.MaxValue)
                {
                    AxisParallelToSquareSideAsInt = new(i, int.MaxValue);
                }
                else
                {
                    AxisParallelToSquareSideAsInt = new(AxisParallelToSquareSideAsInt.Item1, i);
                }
            }
        }

        List<GameObject> SpawnedGameObjects = new();
        Vector3 CurrentElementPosition = positionOfLeftTopCorner;
        for (int i = 0; i < squareSide; i++)
        {
            for (int j = 0; j < squareSide; j++)
            {
                GameObject NewObject = Instantiate(objectTemplate, CurrentElementPosition, Quaternion.identity);
                AudioSource NewAudioSource = NewObject.GetComponent<AudioSource>();
                if (AudioPauser != null && NewAudioSource != null)
                {
                    AudioPauser.AddAudioSourceToRegister(NewAudioSource);
                }
                SpawnedGameObjects.Add(NewObject);
                CurrentElementPosition[AxisParallelToSquareSideAsInt.Item1] += intervalBetweenNeighboringObjects;
            }
            CurrentElementPosition[AxisParallelToSquareSideAsInt.Item1] = positionOfLeftTopCorner[AxisParallelToSquareSideAsInt.Item1];
            CurrentElementPosition[AxisParallelToSquareSideAsInt.Item2] += intervalBetweenNeighboringObjects;
        }
    }
}