using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgain : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(0); // Loads the first scene (Scene 0)
    }
}
