using UnityEngine;
using System.Collections.Generic;

public class ShowingVictory : MonoBehaviour
{
    [SerializeField] private List<GameObject> VictoryMessage = new();

    public void ShowVictory()
    {
        Time.timeScale = 0;
        VictoryMessage.ForEach(oneObject => oneObject.SetActive(true));
    }
}