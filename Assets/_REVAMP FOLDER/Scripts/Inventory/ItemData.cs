using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    StorageItem, ConsumableItem
}

[CreateAssetMenu(menuName = "Items/Item")]

public class ItemData : ScriptableObject
{
    [TextArea (5,5)]
    public string description;
    public Sprite thumbnail;
    public GameObject gameModel;
    public ItemType itemType;

    public int cost;
}
