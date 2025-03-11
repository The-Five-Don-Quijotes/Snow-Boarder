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
            timer?.StopTimer();

            PlayerController player = FindAnyObjectByType<PlayerController>();
            player.DisableControls();
            // Set player base speed and boost to 0
            player.baseSpeed = 0f; 
            player.boostSpeed = 0f;
            // Stop surface effector movement
            SurfaceEffector2D surfaceEffector = FindAnyObjectByType<SurfaceEffector2D>();
            surfaceEffector.speed = 0f; // Stop the player�s movement

            SceneTransition transition = FindFirstObjectByType<SceneTransition>();
            if (transition == null)
            {
                Debug.LogError("SceneTransition object not found!");
            }
            else
            {
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector2.zero;            // Stop all movement
                    rb.angularVelocity = 0f;              // Stop any rotation
                    rb.constraints = RigidbodyConstraints2D.FreezeAll; // Completely freeze the player
                }

                Crash();
                //transition.LoadScene("TryAgain");
            }
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
