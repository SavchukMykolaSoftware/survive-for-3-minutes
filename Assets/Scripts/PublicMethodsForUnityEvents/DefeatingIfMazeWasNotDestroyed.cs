using UnityEngine;

public class DefeatingIfMazeWasNotDestroyed : MonoBehaviour
{
    [SerializeField] private Maze Maze;
    [SerializeField] private ShowingDefeat ShowingDefeat;

    public void DefeatIfMazeWaNotDestroyed()
    {
        if(Maze.WasMazeDestroyed == false)
        {
            ShowingDefeat.LoseAfterShowingImage();
        }
    }
}