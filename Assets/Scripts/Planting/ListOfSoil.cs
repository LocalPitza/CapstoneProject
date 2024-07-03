using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListOfSoil : MonoBehaviour
{
    public static List<ListOfSoil> allSoils = new List<ListOfSoil>();
    public GameObject indicator;
    public Soil soil;
    public PopupTrigger associatedTrigger;

    public static Soil selectedSoil = null;

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
        if (toggle)
        {
            selectedSoil = soil;
        }
    }

    public static void DeselectAll()
    {
        foreach (ListOfSoil soil in allSoils)
        {
            soil.Select(false);
        }
        selectedSoil = null;
    }

    public void InteractWithSelected()
    {
        soil.Interact();
    }
}
