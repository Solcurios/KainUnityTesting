using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public int currentXP = 0;
    public int currentLevel = 1;
    public int xpToNextLevel = 100;

    public LevelUpManager levelUpManager;

    public void AddXP(int amount)
    {
        currentXP += amount;
        if (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentLevel++;
        currentXP -= xpToNextLevel;
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.5f); // Increase XP required each level
        levelUpManager.ShowLevelUpUI();
        // Optional: disable player movement/input here if needed
    }
}
