using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseInventory : MonoBehaviour
{
    public GameObject inventory;

    public void Close()
    {
        inventory.SetActive(false);
    }
}
