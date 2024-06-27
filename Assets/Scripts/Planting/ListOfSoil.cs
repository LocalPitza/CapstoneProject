using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfSoil : MonoBehaviour
{
    public static List<ListOfSoil> allSoils = new List<ListOfSoil>();
    public GameObject indicator;
    public Soil soil;
    public PopupTrigger associatedTrigger;

    private void Awake()
    {
        allSoils.Add(this);
    }

    private void OnDestroy()
    {
        allSoils.Remove(this);
    }

    public void Select(bool toggle)
    {
        indicator.SetActive(toggle);
    }

    public static void DeselectAll()
    {
        foreach (ListOfSoil soil in allSoils)
        {
            soil.Select(false);
        }
    }

    public void InteractWithSelected()
    {
        soil.Interact();
    }
}
