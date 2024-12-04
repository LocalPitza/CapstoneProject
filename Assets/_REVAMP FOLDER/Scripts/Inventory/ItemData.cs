using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Seed,
    StorageItem
}

[CreateAssetMenu(menuName = "Items/Item")]

public class ItemData : ScriptableObject
{
    public string description;
    public Sprite thumbnail;
    public GameObject gameModel;
    public ItemType itemType;
}
