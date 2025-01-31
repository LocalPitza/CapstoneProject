using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreImageAssign : MonoBehaviour
{
    public Image uiImage; // UI Image to update
    public Sprite marketSprite, farmerSprite, storeSprite; // Sprites for each tag
    public Vector3 detectionBoxSize = new Vector3(2f, 2f, 2f); // Size of detection box

    private Dictionary<string, Sprite> spriteDictionary;
    private GameObject detectedStore; // Stores the currently detected store

    private void Start()
    {
        spriteDictionary = new Dictionary<string, Sprite>
        {
            {"Market", marketSprite},
            {"Farmer", farmerSprite},
            {"Store", storeSprite}
        };
    }

    private void Update()
    {
        DetectStore();
        UpdateUIImage();
    }

    private void DetectStore()
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, detectionBoxSize / 2);
        detectedStore = null;

        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Market") || hitCollider.CompareTag("Farmer") || hitCollider.CompareTag("Store"))
            {
                detectedStore = hitCollider.gameObject;
                break; // Stop once a store is found
            }
        }
    }

    private void UpdateUIImage()
    {
        if (detectedStore == null)
        {
            uiImage.enabled = false; // Hide UI if no store is detected
            return;
        }

        uiImage.enabled = true; // Show UI when a store is detected

        if (spriteDictionary.TryGetValue(detectedStore.tag, out Sprite newSprite))
        {
            uiImage.sprite = newSprite;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, detectionBoxSize);
    }
}
