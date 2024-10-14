using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewInventoryManager : MonoBehaviour
{
    public static NewInventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    [Header("Player Tools")]
    public ItemData[] playerTools = new ItemData[5];
    public ItemData equippedTool = null;

    [Header("Seeds UI")]
    public ItemData[] seedsSlots = new ItemData[5];
    public ItemData selectedSeed = null;

    [Header("Storage UI")]
    public ItemData[] storageSlots = new ItemData[15];
    public ItemData selectedStorage = null;
}
