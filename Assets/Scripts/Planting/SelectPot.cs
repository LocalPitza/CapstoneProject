using UnityEngine;
using UnityEngine.EventSystems;

public class SelectPot : MonoBehaviour
{
    void Update()
    {
        Selecting();
    }

    void Selecting()
    {
        if (PopupTrigger.GetActiveTriggers().Count > 0 && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Create a layer mask that includes only the "Pot" layer
            int layerMask = LayerMask.GetMask("Pot");

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                ListOfSoil hitSoil = hit.collider.GetComponent<ListOfSoil>();

                if (hitSoil != null && PopupTrigger.GetActiveTriggers().Contains(hitSoil.associatedTrigger))
                {
                    Deselect(); // Deselect all pots first
                    hitSoil.Select(true); // Select the clicked pot
                    ListOfSoil.selectedSoil = hitSoil.soil;

                    hitSoil.InteractWithSelected();
                }
            }
            else
            {
                Deselect();
            }
        }
    }

    void Deselect()
    {
        ListOfSoil.DeselectAll();
    }
}
