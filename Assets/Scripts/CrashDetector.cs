using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class CrashDetector : MonoBehaviour
{
    [SerializeField] float loadDelay = 0.5f;
    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] AudioClip crashSFX;

    bool hasCrashed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCrashed) return;

        if (other.CompareTag("Ground") || other.CompareTag("Spike"))
        {
            GameTimer timer = UnityEngine.Object.FindFirstObjectByType<GameTimer>();
            if (timer != null)
            {
                timer.StopTimer();
            }
            Crash();
        }
    }

    void Crash()
    {
        hasCrashed = true;
        FindAnyObjectByType<PlayerController>().DisableControls();
        crashEffect.Play();
        GetComponent<AudioSource>().PlayOneShot(crashSFX);
        Invoke("LoadTryAgainScene", loadDelay);
    }

    void LoadTryAgainScene()
    {
        FindFirstObjectByType<SceneTransition>().LoadScene("TryAgain");

        //SceneManager.LoadScene("TryAgain"); // Loads the "Try Again" scene
    }
}
