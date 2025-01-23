using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    public string upgradeKeyPrefix = "Upgrade_"; // Prefix for PlayerPrefs keys
    public List<GameObject> managedObjects; // List of objects to manage

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Replace 'R' with your desired key
        {
            ClearAllPlayerPrefs();
        }
    }

    public void EnableUpgrade(GameObject targetObject)
    {
        string objectKey = upgradeKeyPrefix + targetObject.name; // Use the object's name for saving
        PlayerPrefs.SetInt(objectKey, 1);
        PlayerPrefs.Save();

        if (targetObject != null)
        {
            targetObject.SetActive(true); // Enable the object
        }
        else
        {
            Debug.LogWarning("Target GameObject is null or missing!");
        }
    }

    public void LoadUpgrades()
    {
        foreach (GameObject obj in managedObjects)
        {
            if (obj != null)
            {
                string objectKey = upgradeKeyPrefix + obj.name;
                if (PlayerPrefs.GetInt(objectKey, 0) == 1)
                {
                    obj.SetActive(true); // Enable upgraded object
                }
                else
                {
                    obj.SetActive(false); // Disable by default
                }
            }
            else
            {
                Debug.LogWarning("One of the managed GameObjects is null.");
            }
        }
    }

    private void Awake()
    {
        // Disable all managed objects immediately when the scene is loaded
        foreach (GameObject obj in managedObjects)
        {
            if (obj != null)
            {
                obj.SetActive(false); // Disable by default
            }
        }
    }

    private void Start()
    {
        LoadUpgrades(); // Re-enable purchased objects
    }

    void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs have been cleared.");
    }
}
