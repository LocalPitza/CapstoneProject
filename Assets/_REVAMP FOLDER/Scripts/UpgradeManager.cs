using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeManager : MonoBehaviour
{
    public string upgradeKeyPrefix = "Upgrade_"; // Prefix for PlayerPrefs keys
    public List<GameObject> managedObjects; // List of base objects
    public List<GameObject> upgradedObjects; // List of upgraded versions
    public int upgradeCost = 500;

    private Dictionary<GameObject, GameObject> upgradeMap = new Dictionary<GameObject, GameObject>();

    void Awake()
    {
        if (managedObjects.Count != upgradedObjects.Count)
        {
            Debug.LogError("Managed Objects and Upgraded Objects lists must be the same length!");
            return;
        }

        // Populate dictionary with original → upgraded object mapping
        for (int i = 0; i < managedObjects.Count; i++)
        {
            GameObject oldObject = managedObjects[i];
            GameObject newObject = upgradedObjects[i];

            if (newObject != null)
            {
                if (oldObject != null)
                {
                    upgradeMap[oldObject] = newObject;
                    oldObject.SetActive(true); // Keep original active initially
                }
                else
                {
                    // No old object, but a new object exists
                    upgradeMap[null] = newObject;
                }

                newObject.SetActive(false); // Disable upgrades at start
            }
        }
    }

    void Start()
    {
        LoadUpgrades();
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Replace 'R' with your desired key
        {
            ClearAllPlayerPrefs();
        }
    }*/

    public void EnableUpgrade(GameObject oldObject, GameObject newObject)
    {
        if (PlayerStats.Money < upgradeCost)
        {
            Debug.LogWarning("Not enough money to purchase this upgrade!");
            return;
        }

        if (newObject == null)
        {
            Debug.LogError("Cannot upgrade! No upgraded GameObject assigned.");
            return;
        }

        string objectKey = upgradeKeyPrefix + (oldObject != null ? oldObject.name : newObject.name);

        PlayerStats.Spend(upgradeCost);
        PlayerPrefs.SetInt(objectKey, 1);
        PlayerPrefs.Save();

        if (oldObject != null)
        {
            oldObject.SetActive(false); // ✅ Disable old object
        }

        newObject.SetActive(true); // ✅ Always activate new object
        Debug.Log($"Upgraded to {newObject.name}");
    }

    public void LoadUpgrades()
    {
        foreach (var pair in upgradeMap)
        {
            GameObject oldObject = pair.Key;
            GameObject newObject = pair.Value;

            string objectKey = upgradeKeyPrefix + (oldObject != null ? oldObject.name : newObject.name);
            bool isUpgraded = PlayerPrefs.GetInt(objectKey, 0) == 1;

            if (oldObject != null)
            {
                oldObject.SetActive(!isUpgraded); // ✅ Disable if upgraded
            }

            if (newObject != null)
            {
                newObject.SetActive(isUpgraded); // ✅ Activate if upgraded
            }
        }
    }

    /*void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("All PlayerPrefs have been cleared.");
    }*/
}
