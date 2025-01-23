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

        // Add listener to the button to enable the specific upgrade
        upgradeButton.onClick.AddListener(() => upgradeManager.EnableUpgrade(targetObject));
    }

}
