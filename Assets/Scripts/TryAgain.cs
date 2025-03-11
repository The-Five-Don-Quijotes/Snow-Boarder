using UnityEngine;
using UnityEngine.SceneManagement;

public class TryAgain : MonoBehaviour
{   public void StartGame()
    {
        Debug.Log("Start game");
        SceneManager.LoadScene("Level1");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu Scene");
    }
}
