using UnityEngine;

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
