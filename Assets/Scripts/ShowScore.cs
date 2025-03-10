using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ShowScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;

    void Start()
    {
        int finalScore = PlayerPrefs.GetInt("HighScore", 0); // 0 is the default if no score is found
        scoreText.text = "Final Score: " + finalScore;
    }
}
