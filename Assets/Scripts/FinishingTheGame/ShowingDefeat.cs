using System.Collections.Generic;
using UnityEngine;

public class ShowingDefeat : MonoBehaviour
{
    [SerializeField] private GameObject DefeatScreen;
    [SerializeField] private List<GameObject> ObjectsWhichShouldBeDisabledAfterLosing = new();
    [SerializeField] private ImageWhichChangesTransparencyUniformly ImageToBeShowedBeforeLosing;
    [SerializeField] private float TimeForShowingImageBeforeLosing = 1;

    public void Lose()
    {
        Time.timeScale = 0;
        DefeatScreen.SetActive(true);
        ObjectsWhichShouldBeDisabledAfterLosing.ForEach(gameObject => gameObject.SetActive(false));
    }

    public void LoseAfterShowingImage()
    {
        ImageToBeShowedBeforeLosing.ShowGradually(TimeForShowingImageBeforeLosing, () =>
        {
            ImageToBeShowedBeforeLosing.gameObject.SetActive(false);
            Lose();
        });
    }
}