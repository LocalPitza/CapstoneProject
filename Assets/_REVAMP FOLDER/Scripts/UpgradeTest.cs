using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeTest : MonoBehaviour
{
    public GameObject targetObject; // The GameObject to upgrade
    public Button upgradeButton; // Button assigned for this upgrade
    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager not found in the scene.");
            return; // Stop execution if UpgradeManager is missing
        }

        if (upgradeButton == null)
        {
            Debug.LogError("Upgrade button not assigned for " + targetObject.name);
            return; // Stop execution if no button is assigned
        }

        // Check upgrade status and update button interactability
        UpdateButtonState();

        // Add listener to the button to enable the specific upgrade
        upgradeButton.onClick.AddListener(() =>
        {
            upgradeManager.EnableUpgrade(targetObject);
            UpdateButtonState(); // Update the button's interactability after the upgrade
        });
    }

    private void UpdateButtonState()
    {
        // Check if the upgrade is already purchased
        string objectKey = upgradeManager.upgradeKeyPrefix + targetObject.name;
        bool isUpgraded = PlayerPrefs.GetInt(objectKey, 0) == 1;

        // Disable the button if the upgrade is already purchased
        upgradeButton.interactable = !isUpgraded;
    }

}
