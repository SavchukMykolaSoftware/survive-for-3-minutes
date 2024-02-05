using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour
{
    public void Restart()
    {
        LoadSceneByName(SceneManager.GetActiveScene().name);
    }

    public void LoadSceneByName(string nameOfSceneToBeLoaded)
    {
        SceneManager.LoadScene(nameOfSceneToBeLoaded);
    }
}