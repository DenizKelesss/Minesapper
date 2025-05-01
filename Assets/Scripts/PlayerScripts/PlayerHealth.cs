using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;

    public Slider healthBar;
    private UIManager uiManager;

    private void Start()
    {
        currentHealth = maxHealth;
        uiManager = FindFirstObjectByType<UIManager>();

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
    }

    public void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Max(currentHealth, 0);

        Debug.Log("Player Health: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }

        if (currentHealth <= 0 && uiManager != null)
        {
            uiManager.RestartGame();
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
