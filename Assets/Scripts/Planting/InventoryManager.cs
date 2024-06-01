using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //To avoid having multiple Instance of this Script
    public static InventoryManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    // // // //

    [Header ("Tools")]
    //Tool Slots
    public ItemData[] tools = new ItemData[4];
    //Tool on Hand
    public ItemData equipedTool = null;

    [Header ("Items")]
    //Item Slots
    public ItemData[] items = new ItemData[4];
    //Item on Hand
    public ItemData equipedItem = null;
}
