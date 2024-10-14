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

    [Header("Tools")]
    // Tool Slots
    public ItemData[] tools = new ItemData[8];
    // Tool on Hand
    public ItemData equippedTool = null;
}
