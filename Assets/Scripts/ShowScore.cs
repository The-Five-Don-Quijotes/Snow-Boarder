using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShowScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI finalTimeText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("HighScore", 0); // 0 is the default if no score is found
        scoreText.text = "Final Score: " + finalScore;

        string finalTime = PlayerPrefs.GetString("FinalTime", "00:00");
        finalTimeText.text = finalTime;
    }
}
