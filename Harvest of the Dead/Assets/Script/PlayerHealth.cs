using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float maxHealth = 100f;
    private float currentHealth;

    [Header("UI Settings")]
    public GameObject deathUI; // Assign a UI panel with "YOU DIED" and "Tap to Continue"
    public Slider healthBar;   // Optional health bar UI

    [Header("References")]
    public Animator animator;   // Assign your player's Animator here
    public MonoBehaviour[] scriptsToDisable; // e.g., movement or shooting scripts

    private bool isDead = false;

    void Start()
    {
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
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Play death animation
        if (animator != null)
            animator.SetTrigger("Die");

        // Disable all player control scripts (movement, shooting, etc.)
        foreach (var script in scriptsToDisable)
        {
            if (script != null)
                script.enabled = false;
        }

        // Show death UI
        if (deathUI != null)
            deathUI.SetActive(true);

        // Pause everything after short delay (allow animation to play)
        Invoke("ShowDeathScreen", 2f);
    }

    void ShowDeathScreen()
    {
        Time.timeScale = 0f; // Pause game
    }

    // Button callback on death screen
    public void OnRetryButton()
    {
        Time.timeScale = 1f; // Resume game
        SceneManager.LoadScene("MainMenu"); // Change to your Main Menu scene name
    }
}
