using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public int playerXP = 0;
    public int playerHealthLevel = 1;
    public int failDamageLevel = 1;
    public int destroyChanceLevel = 1;
    public int currentLevel = 1;  // Add this to track the current level

    public TextMeshProUGUI playerXPText;
    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI currentLevelText;  // Display current level in the UI

    public int xpPerUpgrade = 10;

    public PlayerHealth playerHealth;
    public MineDestructor mineDestructor;
    public int baseMaxHealth = 10;
    public int baseFailDamage = 3;
    public float baseDestroyChance = 50f;

    private void Start()
    {
        LoadProgress();  // Load all progress data, including level
        ApplyUpgrades();
        UpdateUI();
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerXP", playerXP);
        PlayerPrefs.SetInt("PlayerHealthLevel", playerHealthLevel);
        PlayerPrefs.SetInt("FailDamageLevel", failDamageLevel);
        PlayerPrefs.SetInt("DestroyChanceLevel", destroyChanceLevel);
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);  // Save current level
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        playerXP = PlayerPrefs.GetInt("PlayerXP", 0);
        playerHealthLevel = PlayerPrefs.GetInt("PlayerHealthLevel", 1);
        failDamageLevel = PlayerPrefs.GetInt("FailDamageLevel", 1);
        destroyChanceLevel = PlayerPrefs.GetInt("DestroyChanceLevel", 1);
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);  // Load current level
    }

    public void GainXP(int amount)
    {
        playerXP += amount;
        Debug.Log("XP Gained: " + amount + ". Total XP: " + playerXP);
        UpdateUI();
        SaveProgress();
    }

    private void UpdateUI()
    {
        if (playerXPText != null)
            playerXPText.text = "XP: " + playerXP;

        if (playerLevelText != null)
            playerLevelText.text = "Health Level: " + playerHealthLevel;

        if (currentLevelText != null)
            currentLevelText.text = "Level: " + currentLevel;  // Show the current level in the UI
    }

    public void UpgradePlayerHealth()
    {
        if (playerXP >= xpPerUpgrade)
        {
            playerXP -= xpPerUpgrade;
            playerHealthLevel++;
            ApplyUpgrades();
            UpdateUI();
            SaveProgress();
        }
    }

    public void UpgradeFailDamage()
    {
        if (playerXP >= xpPerUpgrade)
        {
            playerXP -= xpPerUpgrade;
            failDamageLevel++;
            ApplyUpgrades();
            UpdateUI();
            SaveProgress();
        }
    }

    public void UpgradeDestroyChance()
    {
        if (playerXP >= xpPerUpgrade)
        {
            playerXP -= xpPerUpgrade;
            destroyChanceLevel++;
            ApplyUpgrades();
            UpdateUI();
            SaveProgress();
        }
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("PlayerXP");
        PlayerPrefs.DeleteKey("PlayerHealthLevel");
        PlayerPrefs.DeleteKey("FailDamageLevel");
        PlayerPrefs.DeleteKey("DestroyChanceLevel");
        PlayerPrefs.DeleteKey("CurrentLevel");  // Also reset the current level
        PlayerPrefs.Save();
    }

    private void ApplyUpgrades()
    {
        if (playerHealth != null)
            playerHealth.maxHealth = baseMaxHealth + (playerHealthLevel - 1) * 2;

        if (mineDestructor != null)
            mineDestructor.destroyChance = baseDestroyChance + (destroyChanceLevel - 1) * 5f;
    }

    public int GetFailDamage()
    {
        return Mathf.Max(1, baseFailDamage - (failDamageLevel - 1));
    }

    // Add methods to handle leveling up or changing levels
    public void LevelUp()
    {
        currentLevel++;
        SaveProgress();
    }

    public void SetLevel(int level)
    {
        currentLevel = level;
        SaveProgress();
    }
}
