using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment")]
public class EquipmentData : ItemData //Inherits the Fields of the Class
{
    public enum ToolType
    {
        HandTrowel, PruningShears, WateringCan
    }

    public ToolType toolType;
}
