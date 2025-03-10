using UnityEngine;
using TMPro;

public class CalculateScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] AudioSource audioSource; // Reference to AudioSource
    [SerializeField] AudioClip scoreGainSound; // The score gain sound

    int score = 0;

    void Start()
    {
        PlayerPrefs.SetInt("HighScore", score); // Save the score to PlayerPrefs
        PlayerPrefs.Save();
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreText();
        PlayScoreSound();
        PlayerPrefs.SetInt("HighScore", score); // Save the score to PlayerPrefs
        PlayerPrefs.Save();
    }

    void UpdateScoreText()
    {
        scoreText.text = "Score: " + score;
    }

    void PlayScoreSound()
    {
        if (audioSource != null && scoreGainSound != null)
        {
            audioSource.clip = scoreGainSound;
            audioSource.pitch = Random.Range(0.8f, 1.2f); // Randomize pitch between 0.8 and 1.2
            audioSource.Play();
        }
    }
}
