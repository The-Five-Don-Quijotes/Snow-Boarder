using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Start game");
        SceneManager.LoadScene("Level1");
    }

    public void ShowInstruction()
    {
        Debug.Log("Instruction");
        SceneManager.LoadScene("Instruction Scene");
    }

    public void MenuScene()
    {
        SceneManager.LoadScene("Menu Scene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
