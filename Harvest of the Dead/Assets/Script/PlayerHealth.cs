using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Settings")]
    public GameObject deathUI; // assign your "You Died" panel
    public Slider healthBar;   // assign your health bar slider

    [Header("References")]
    public Animator animator;  // assign your player animator

    private bool isDead = false;

    void Start()
    {
        // Ensure game runs normally if loaded after death
        Time.timeScale = 1f;

        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = maxHealth;
        }

        if (deathUI != null)
            deathUI.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        if (isDead) return;

        currentHealth -= damage;

        if (healthBar != null)
            healthBar.value = currentHealth;

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // 🔹 Trigger death animation
        if (animator != null)
            animator.SetTrigger("Die");

        // 🔹 Disable all scripts except this one
        MonoBehaviour[] allScripts = GetComponentsInChildren<MonoBehaviour>();
        foreach (MonoBehaviour script in allScripts)
        {
            if (script != this)
                script.enabled = false;
        }

        // 🔹 Stop all audio
        AudioSource[] allAudioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Stop();
        }

        // 🔹 Unlock mouse + show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 🔹 Show death UI after a short delay so animation can play
        Invoke(nameof(ShowDeathScreen), 1.5f);
    }

    void ShowDeathScreen()
    {
        if (deathUI != null)
            deathUI.SetActive(true);

        Time.timeScale = 0f; // freeze game
    }

    // 🔹 Called from "Tap to Continue" button
    public void OnRetryButton()
    {
        Time.timeScale = 1f; // unfreeze
        SceneManager.LoadScene("MainMenu"); // replace with your actual scene name
    }
}
