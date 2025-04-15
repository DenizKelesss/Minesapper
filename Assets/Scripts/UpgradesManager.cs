using UnityEngine;
using TMPro;

public class UpgradeManager : MonoBehaviour
{
    public int playerXP = 0;
    public int playerHealthLevel = 1;
    public int failDamageLevel = 1;
    public int destroyChanceLevel = 1;

    public TextMeshProUGUI playerXPText;
    public TextMeshProUGUI playerLevelText;


    public int xpPerUpgrade = 10;

    public PlayerHealth playerHealth;
    public MineDestructor mineDestructor;
    public int baseMaxHealth = 10;
    public int baseFailDamage = 3;
    public float baseDestroyChance = 50f;

    private void Start()
    {
        LoadProgress();
        ApplyUpgrades();
        UpdateUI();
    }

    public void SaveProgress()
    {
        PlayerPrefs.SetInt("PlayerXP", playerXP);
        PlayerPrefs.SetInt("PlayerHealthLevel", playerHealthLevel);
        PlayerPrefs.SetInt("FailDamageLevel", failDamageLevel);
        PlayerPrefs.SetInt("DestroyChanceLevel", destroyChanceLevel);
        PlayerPrefs.Save();
    }

    public void LoadProgress()
    {
        playerXP = PlayerPrefs.GetInt("PlayerXP", 0);
        playerHealthLevel = PlayerPrefs.GetInt("PlayerHealthLevel", 1);
        failDamageLevel = PlayerPrefs.GetInt("FailDamageLevel", 1);
        destroyChanceLevel = PlayerPrefs.GetInt("DestroyChanceLevel", 1);
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

        // If you have a level text, update it too
        if (playerLevelText != null)
            playerLevelText.text = "Level: " + playerHealthLevel;
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
}
