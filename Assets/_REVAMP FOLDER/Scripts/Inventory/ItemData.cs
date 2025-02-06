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
    public string description;
    public Sprite thumbnail;
    public GameObject gameModel;
    public ItemType itemType;

    public int cost;
    public int hungerRefill;
    public int energyRefill;

    public bool needIngredient;
    public ItemData requiredIngredient;
}
