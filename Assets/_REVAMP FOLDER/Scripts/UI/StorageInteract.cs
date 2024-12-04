using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageInteract : MonoBehaviour
{
    [SerializeField] GameObject[] storageUI;
    [SerializeField] Button toggleButton; // Reference to the toggle button
    private InteractMessage interactMessage;

    void Start()
    {
        foreach (GameObject ui in storageUI)
        {
            if (ui != null)
                ui.SetActive(false);
        }

        interactMessage = GetComponent<InteractMessage>();

        if (toggleButton != null)
            toggleButton.interactable = true; // Ensure the button is enabled initially
    }

    private void Update()
    {
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(KeyCode.F))
        {
            bool isActive = false;

            foreach (GameObject ui in storageUI)
            {
                if (ui != null)
                {
                    ui.SetActive(!ui.activeSelf);
                    isActive = ui.activeSelf; // Determine the final state
                }
            }

            // Disable the button if the UI is open, otherwise enable it
            if (toggleButton != null)
                toggleButton.interactable = !isActive;

            NewUIManager.Instance.RenderInventory();
        }
    }

    public bool IsUIActive()
    {
        foreach (GameObject ui in storageUI)
        {
            if (ui != null && ui.activeSelf)
                return true;
        }
        return false;
    }
}
