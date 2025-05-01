using UnityEngine;
using TMPro;
using System.IO;

public class UpgradeManager : MonoBehaviour
{
    public int playerXP = 0;
    public int playerHealthLevel = 1;
    public int failDamageLevel = 1;
    public int destroyChanceLevel = 1;
    public int currentLevel = 1;

    public TextMeshProUGUI playerXPText;
    public TextMeshProUGUI playerHealthLevelText;
    public TextMeshProUGUI playerResistanceLevelText;
    public TextMeshProUGUI playerMineDestroyChanceLevelText;


    public TextMeshProUGUI currentLevelText;

    public int xpPerUpgrade = 10;

    public PlayerHealth playerHealth;
    public MineDestructor mineDestructor;
    public int baseMaxHealth = 10;
    public int baseFailDamage = 3;
    public float baseDestroyChance = 50f;

    private void Start()
    {
        LoadProgress();  // Load all progress data, including level data, update data (faildamageleve, healthlevel, destroy chance leve) and playerXP data - health might be needed too?
        ApplyUpgrades();
        UpdateUI();
    }

    public void SaveProgress()
    {
        SaveData data = new SaveData
        {
            playerXP = playerXP,
            playerHealthLevel = playerHealthLevel,
            failDamageLevel = failDamageLevel,
            destroyChanceLevel = destroyChanceLevel,
            currentLevel = currentLevel
        };

        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadProgress()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerXP = data.playerXP;
            playerHealthLevel = data.playerHealthLevel;
            failDamageLevel = data.failDamageLevel;
            destroyChanceLevel = data.destroyChanceLevel;
            currentLevel = data.currentLevel;
        }
        else
        {
            playerXP = 0;
            playerHealthLevel = 1;
            failDamageLevel = 1;
            destroyChanceLevel = 1;
            currentLevel = 1;
        }
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

        if (playerHealthLevelText != null)
            playerHealthLevelText.text = "Health Level: " + playerHealthLevel;

        if (playerResistanceLevelText != null)
            playerResistanceLevelText.text = "Damage Resistance Level: " + failDamageLevel;

        if (playerMineDestroyChanceLevelText != null)
            playerMineDestroyChanceLevelText.text = "Damage Resistance Level: " + destroyChanceLevel;

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
        string path = Application.persistentDataPath + "/savefile.json";
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
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
