using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene transition

public class GameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText; // UI Text for displaying the timer
    private float startTime;
    private bool isRunning = true;
    private float elapsedTime = 0f;

    void Start()
    {
        startTime = Time.time;
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60);
            int seconds = Mathf.FloorToInt(elapsedTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Call this when the player finishes or crashes
    public void StopTimer()
    {
        if (!isRunning) return; // Prevent multiple calls

        isRunning = false;
        string finalTime = timerText.text;
        PlayerPrefs.SetString("FinalTime", finalTime);
        PlayerPrefs.Save();
        Debug.Log("Final Time: " + timerText.text);
    }
}
