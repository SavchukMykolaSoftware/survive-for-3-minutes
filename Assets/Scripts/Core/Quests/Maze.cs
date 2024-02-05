using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using MazeAlgorithms;

public class Maze : MonoBehaviour
{
    [Header("Size of the maze")]
    [SerializeField] private int Width;
    [SerializeField] private int Length;

    [Header("Visualizing")]
    [SerializeField] private float OneBlockWidthInTheScene = 1;
    [SerializeField] private GameObject SampleBlock;
    [SerializeField] private GameObject SampleFloorForEmptyFields;
    [SerializeField] private GameObject ParentObjectForMapSigns;
    [SerializeField] private GameObject SampleBlockSignForMap;
    [SerializeField] private GameObject PlayerSignForMap;
    [SerializeField] private GameObject SamplePathFieldSign;
    [SerializeField] private Button ButtonForShowingPath;
    [SerializeField] private float TimeForShowingPath = 3;
    [SerializeField] private GameObject SampleTargetSignForMap;
    [SerializeField] private GameObject ThePlayer;
    [SerializeField] private GameObject TheTarget;
    [SerializeField] private float TargetOrdinate;
    [SerializeField] private Vector3 LeftBackCornerPositionInTheScene;

    [Header("Destroying the maze")]
    [SerializeField] private MaterialsWhichChangeTransparencyUniformly MaterialsOfTheMazeToBeMadeTransparent;
    [SerializeField] private float TimeForDestroyingMaze;

    private List<List<bool>> TheMaze;
    private Vector2 TargetPositionInTheMaze = Vector2.zero;
    private Vector2 LeftBackCornerPositionOnTheMap;
    private float OneSignWidthOnTheMap = 10;
    private Vector3 LeftBackCornerOfLeftBackCornerBlockPosition;
    private Vector3 RightForwardCornerOfRightForwardCornerBlockPosition;
    private Vector2 LeftBackCornerOfLeftBackCornerBlockSignPosition;
    private Vector2 RightForwardCornerOfRightForwardCornerBlockSignPosition;
    private Transform PlayerTransform;
    private RectTransform RectTransformOfPlayerSign;
    private List<GameObject> AllPathSignsOnTheMap = new();
    private List<GameObject> AllMazeElements;

    public bool WasMazeDestroyed { get; private set; } = false;

    private void Start()
    {
        void InstantiateFloor(Vector3 currentPositionInTheScene)
        {
            AllMazeElements.Add(InstatiateObjectAtPosition(SampleFloorForEmptyFields, currentPositionInTheScene - OneBlockWidthInTheScene / 2 * Vector3.up));
        }
        void InstantiateCeiling(Vector3 currentPositionInTheScene)
        {
            GameObject CeilingForThisField = InstatiateObjectAtPosition(SampleFloorForEmptyFields, currentPositionInTheScene + OneBlockWidthInTheScene / 2 * Vector3.up);
            CeilingForThisField.transform.rotation *= Quaternion.Euler(0, 0, 180);
            AllMazeElements.Add(CeilingForThisField);
        }
        
        AllMazeElements = new() { SampleBlockSignForMap, PlayerSignForMap, SampleTargetSignForMap, ButtonForShowingPath.gameObject, TheTarget };
        AllMazeElements.ForEach((oneObject) => oneObject.SetActive(true));

        PlayerTransform = ThePlayer.transform;

        RectTransformOfPlayerSign = PlayerSignForMap.GetComponent<RectTransform>();

        RectTransform RectTransformOfBlockSign = SampleBlockSignForMap.GetComponent<RectTransform>();
        OneSignWidthOnTheMap = RectTransformOfBlockSign.sizeDelta.x / RectTransformOfBlockSign.localScale.x;
        LeftBackCornerPositionOnTheMap = RectTransformOfBlockSign.anchoredPosition;

        LeftBackCornerOfLeftBackCornerBlockPosition = LeftBackCornerPositionInTheScene - new Vector3(OneBlockWidthInTheScene, 0, OneBlockWidthInTheScene) / 2;
        RightForwardCornerOfRightForwardCornerBlockPosition = LeftBackCornerOfLeftBackCornerBlockPosition + new Vector3(OneBlockWidthInTheScene * Width, 0, OneBlockWidthInTheScene * Length);
        LeftBackCornerOfLeftBackCornerBlockSignPosition = LeftBackCornerPositionOnTheMap - (RectTransformOfBlockSign.sizeDelta.x / RectTransformOfBlockSign.localScale.x) * Vector2.one * 0.5f;
        RightForwardCornerOfRightForwardCornerBlockSignPosition = LeftBackCornerPositionOnTheMap + new Vector2((RectTransformOfBlockSign.sizeDelta.x / RectTransformOfBlockSign.localScale.x) * (Width - 0.5f), (RectTransformOfBlockSign.sizeDelta.x / RectTransformOfBlockSign.localScale.x) * (Length - 0.5f));

        TheMaze = new RandomMazeGenerator().GenerateMaze(Width - 2, Length - 2);

        TheMaze.Insert(0, new());
        TheMaze.Add(new());
        for (int i = 0; i < Width; i++)
        {
            TheMaze[0].Add(false);
            TheMaze[TheMaze.Count - 1].Add(false);
        }
        for (int i = 0; i < Length; i++)
        {
            TheMaze[i].Insert(0, false);
            TheMaze[i].Add(false);
        }

        TargetPositionInTheMaze = Vector2.zero;
        Vector2 PlayerPosition = Vector2.zero;
        bool WasPlayerPositionSet = false;
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (TheMaze[i][j])
                {
                    if (!WasPlayerPositionSet)
                    {
                        PlayerPosition = new(i, j);
                        WasPlayerPositionSet = true;
                    }
                    TargetPositionInTheMaze = new(i, j);
                }
            }
        }

        KeyValuePair<int, int> PlayerPositionAsKeyValuePair = new((int)PlayerPosition.x, (int)PlayerPosition.y);
        KeyValuePair<int, int> TargetPositionAsKeyValuePair = new((int)TargetPositionInTheMaze.x, (int)TargetPositionInTheMaze.y);

        List<KeyValuePair<int, int>> AllEmptyPositions = new();
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (TheMaze[i][j] && !(PlayerPosition.x == i && PlayerPosition.y == j) && !(TargetPositionInTheMaze.x == i && TargetPositionInTheMaze.y == j))
                {
                    AllEmptyPositions.Add(new(i, j));
                }
            }
        }

        ParentObjectForMapSigns.SetActive(true);
        Vector3 CurrentPositionInTheScene = LeftBackCornerPositionInTheScene;
        Vector3 CurrentPositionOnTheMap = LeftBackCornerPositionOnTheMap;
        for (int i = 0; i < Length; i++)
        {
            for (int j = 0; j < Width; j++)
            {
                if (!TheMaze[i][j])
                {
                    AllMazeElements.Add(InstatiateObjectAtPosition(SampleBlock, CurrentPositionInTheScene));
                    if (i != 0 || j != 0)
                    {
                        AllMazeElements.Add(PutSignToTheMap(SampleBlockSignForMap, CurrentPositionOnTheMap));
                    }
                }
                else
                {
                    InstantiateFloor(CurrentPositionInTheScene);
                    InstantiateCeiling(CurrentPositionInTheScene);
                    if (TargetPositionInTheMaze.x == i && TargetPositionInTheMaze.y == j)
                    {
                        TheTarget.transform.position = new(CurrentPositionInTheScene.x, TargetOrdinate, CurrentPositionInTheScene.z);
                        SampleTargetSignForMap.GetComponent<RectTransform>().anchoredPosition = CurrentPositionOnTheMap;
                    }
                    else if (PlayerPosition.x == i && PlayerPosition.y == j)
                    {
                        //PlayerTransform = InstatiateObjectAtPosition(Player, CurrentPositionInTheScene).transform;
                        ThePlayer.transform.position = CurrentPositionInTheScene;
                    }
                }
                CurrentPositionInTheScene.z += OneBlockWidthInTheScene;
                CurrentPositionOnTheMap.y += OneSignWidthOnTheMap;
            }
            CurrentPositionInTheScene.x += OneBlockWidthInTheScene;
            CurrentPositionOnTheMap.x += OneSignWidthOnTheMap;
            CurrentPositionInTheScene.z = LeftBackCornerPositionInTheScene.z;
            CurrentPositionOnTheMap.y = LeftBackCornerPositionOnTheMap.y;
        }

        PlayerSignForMap.GetComponent<RectTransform>().SetAsLastSibling();
        ButtonForShowingPath.onClick.AddListener(() =>
        {
            FindAndShowPathFromPlayerToTarget();
            Invoke(nameof(DeleteShowedPath), TimeForShowingPath);
            ButtonForShowingPath.gameObject.SetActive(false);
        });
    }

    private GameObject InstatiateObjectAtPosition(GameObject sample, Vector3 position)
    {
        GameObject NewGameObject = Instantiate(sample);
        NewGameObject.transform.position = position;
        return NewGameObject;
    }

    private GameObject PutSignToTheMap(GameObject sample, Vector2 anchoredPosition)
    {
        GameObject NewSign = Instantiate(sample);
        NewSign.SetActive(true);
        RectTransform RectTransformOfNewSign = NewSign.GetComponent<RectTransform>();
        RectTransformOfNewSign.SetParent(ParentObjectForMapSigns.transform);
        RectTransformOfNewSign.anchoredPosition = anchoredPosition;
        RectTransformOfNewSign.sizeDelta /= RectTransformOfNewSign.localScale.x;
        return NewSign;
    }

    private void RecalculatePlayerPositionRelativeToMazeSize(out float ratioBetweenDistanceFromPlayerToCornerAndMazeWidth, out float ratioBetweenDistanceFromPlayerToCornerAndMazeLength)
    {
        float CalculateRatioInOneAxis(Axis axis)
        {
            int AxisAsInt = (int)axis;
            return (PlayerTransform.transform.position[AxisAsInt] - LeftBackCornerOfLeftBackCornerBlockPosition[AxisAsInt])
            / (RightForwardCornerOfRightForwardCornerBlockPosition[AxisAsInt] - LeftBackCornerOfLeftBackCornerBlockPosition[AxisAsInt]);
        }

        ratioBetweenDistanceFromPlayerToCornerAndMazeWidth = CalculateRatioInOneAxis(Axis.X);
        ratioBetweenDistanceFromPlayerToCornerAndMazeLength = CalculateRatioInOneAxis(Axis.Z);
    }

    private void PrintPlayerPositionOnTheMap()
    {
        RecalculatePlayerPositionRelativeToMazeSize(out float RatioBetweenDistanceFromPlayerToCornerAndMazeWidth, out float RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

        Vector2 NewPositionOfPlayerSignOnTheMap =
        new((LeftBackCornerOfLeftBackCornerBlockSignPosition.x + (RightForwardCornerOfRightForwardCornerBlockSignPosition.x - LeftBackCornerOfLeftBackCornerBlockSignPosition.x) * RatioBetweenDistanceFromPlayerToCornerAndMazeWidth),
        LeftBackCornerOfLeftBackCornerBlockSignPosition.y + (RightForwardCornerOfRightForwardCornerBlockSignPosition.y - LeftBackCornerOfLeftBackCornerBlockSignPosition.y) * RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

        RectTransformOfPlayerSign.anchoredPosition = NewPositionOfPlayerSignOnTheMap;
    }

    private void Update()
    {
        if (WasMazeDestroyed == false)
        {
            PrintPlayerPositionOnTheMap();
        }
    }

    public void FindAndShowPathFromPlayerToTarget()
    {
        if (AllPathSignsOnTheMap.Count == 0)
        {
            RecalculatePlayerPositionRelativeToMazeSize(out float RatioBetweenDistanceFromPlayerToCornerAndMazeWidth, out float RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

            int AbscissaOfCurrentPlayerPositionInTheMaze = (int)(Width * RatioBetweenDistanceFromPlayerToCornerAndMazeWidth);
            int OrdinateOfCurrentPlayerPositionInTheMaze = (int)(Length * RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

            List<KeyValuePair<int, int>> PathFromPlayerToTarget = new PathFinder().FindPathBetweenTwoPoints(TheMaze, new(AbscissaOfCurrentPlayerPositionInTheMaze, OrdinateOfCurrentPlayerPositionInTheMaze), new((int)TargetPositionInTheMaze.x, (int)TargetPositionInTheMaze.y));

            for (int i = 0; i < PathFromPlayerToTarget.Count; i++)
            {
                AllPathSignsOnTheMap.Add(PutSignToTheMap(SamplePathFieldSign, new Vector2(LeftBackCornerPositionOnTheMap.x + PathFromPlayerToTarget[i].Key * OneSignWidthOnTheMap, LeftBackCornerPositionOnTheMap.y + PathFromPlayerToTarget[i].Value * OneSignWidthOnTheMap)));
            }
        }
    }

    private void DeleteShowedPath()
    {
        for (int i = AllPathSignsOnTheMap.Count - 1; i >= 0; i--)
        {
            Destroy(AllPathSignsOnTheMap[i]);
        }
        AllPathSignsOnTheMap.Clear();
    }

    public void DestroyWholeMaze()
    {
        MaterialsOfTheMazeToBeMadeTransparent.StartChangingTransparency(1, 0, TimeForDestroyingMaze, () =>
        {
            DeleteShowedPath();
            AllMazeElements.ForEach(Destroy);
        });
        WasMazeDestroyed = true;
    }
}