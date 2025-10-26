using UnityEngine;
using UnityEngine.UI; // Required for using UI components like Slider

public class PlayerHealth : MonoBehaviour
{
    // MUST be assigned to your UI Slider in the Inspector
    public Slider healthBarSlider;

    // Max health set to 6 for the 6-hit-kill rule (since damage is 1)
    public const int MaxHealth = 6;
    private int currentHealth;

    void Start()
    {
        currentHealth = MaxHealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = MaxHealth;
            healthBarSlider.value = currentHealth;
        }
    }

    /// <summary>
    /// Reduces player health and checks for death.
    /// Called by the ZombieAI script via the Animation Event.
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;

        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        Debug.Log("Player HIT! Remaining Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died!");

        // Disable player controls
        PlayerController pc = GetComponent<PlayerController>();
        if (pc != null) pc.enabled = false;

        // Add death animation/game over screen logic here
    }
}