using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
    public GameObject levelUpPanel;
    public Button[] upgradeButtons;
    public MonoBehaviour playerController; // drag FirstPersonController or your script

    private bool isLevelUpActive = false;
    private List<System.Action> availableUpgrades;

    void Start()
    {
        availableUpgrades = new List<System.Action>()
        {
            UpgradeDamage,
            UpgradeFireRate,
            UpgradeHealth
        };

        HideLevelUpUI();
    }

public void ShowLevelUpUI()
{
    if (isLevelUpActive) return; // prevent re-activating

    Time.timeScale = 0f;
    isLevelUpActive = true;
    levelUpPanel.SetActive(true);

    Cursor.lockState = CursorLockMode.None;
    Cursor.visible = true;

    if (playerController != null) playerController.enabled = false;

    for (int i = 0; i < upgradeButtons.Length; i++)
    {
        if (i < availableUpgrades.Count)
        {
            int copy = i;
            upgradeButtons[i].gameObject.SetActive(true);
            upgradeButtons[i].onClick.RemoveAllListeners();
            upgradeButtons[i].onClick.AddListener(() => ChooseUpgrade(availableUpgrades[copy]));
            upgradeButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = GetUpgradeName(availableUpgrades[copy]);
        }
        else
        {
            upgradeButtons[i].gameObject.SetActive(false);
        }
    }
}

    void HideLevelUpUI()
    {
        levelUpPanel.SetActive(false);
        isLevelUpActive = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (playerController != null) playerController.enabled = true;
    }

    void ChooseUpgrade(System.Action upgrade)
    {
        upgrade.Invoke();
        HideLevelUpUI();
    }

    // Example upgrade implementations
    void UpgradeDamage()
    {
        Debug.Log("Damage Upgraded!");
        // e.g. Increase player damage, for example:
        Object.FindAnyObjectByType<Weapon>().damage += 10;
    }

    void UpgradeFireRate()
    {
        Debug.Log("Fire Rate Upgraded!");
        Weapon weapon = Object.FindAnyObjectByType<Weapon>();
        weapon.fireRate = Mathf.Max(0.1f, weapon.fireRate - 0.1f);
    }

    void UpgradeHealth()
    {
        Debug.Log("Health Upgraded!");
        PlayerHealth playerHealth = Object.FindAnyObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.maxHealth += 20;
            playerHealth.currentHealth += 20;
        }
    }

    // Helper to get upgrade name for button text
    string GetUpgradeName(System.Action upgrade)
    {
        if (upgrade == UpgradeDamage) return "Increase Damage";
        if (upgrade == UpgradeFireRate) return "Increase Fire Rate";
        if (upgrade == UpgradeHealth) return "Increase Health";
        return "Upgrade";
    }
}
