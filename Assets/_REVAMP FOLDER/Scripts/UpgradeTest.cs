using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UpgradeTest : MonoBehaviour
{
    public GameObject targetObject; // The original GameObject
    public GameObject upgradedObject;
    public Button upgradeButton; // Button assigned for this upgrade
    private UpgradeManager upgradeManager;

    void Start()
    {
        upgradeManager = FindObjectOfType<UpgradeManager>();
        if (upgradeManager == null)
        {
            Debug.LogError("UpgradeManager not found in the scene.");
            return;
        }

        if (upgradeButton == null)
        {
            Debug.LogError("Upgrade button not assigned.");
            return;
        }

        if (upgradedObject == null)
        {
            Debug.LogError("Upgraded object is missing for this upgrade.");
            return;
        }

        // Check upgrade status and update button interactability
        UpdateButtonState();

        // Add listener to the button to enable the specific upgrade
        upgradeButton.onClick.AddListener(() =>
        {
            upgradeManager.EnableUpgrade(targetObject, upgradedObject);
            UpdateButtonState(); // Update the button's interactability after the upgrade
        });
    }

    private void UpdateButtonState()
    {
        string objectKey = upgradeManager.upgradeKeyPrefix + (targetObject != null ? targetObject.name : upgradedObject.name);
        bool isUpgraded = PlayerPrefs.GetInt(objectKey, 0) == 1;
        upgradeButton.interactable = !isUpgraded;
    }

}
