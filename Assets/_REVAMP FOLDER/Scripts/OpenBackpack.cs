using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBackpack : MonoBehaviour
{
    public GameObject[] backpackUI;

    public Image bagIcon; // The UI Image that will change
    public Sprite[] bagSprites; // 0 = Closed, 1 = Open

    private bool isOpen = false;

    private void Start()
    {
        foreach (GameObject panel in backpackUI)
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }
        UpdateBagIcon(); // Set initial icon
    }
    private void Update()
    {
        // Prevent opening if multiple UIs are already open
        if (CursorManager.Instance.GetUIOpenCount() > 1) return;

        if (Input.GetKeyDown(InputManager.Instance.openBackpack))
        {
            FindObjectOfType<SoundManager>().Play("SNBClick");
            ToggleBackpackPanel();
        }
    }

    public void ToggleBackpackPanel()
    {
        // If multiple UIs are open, don't allow opening
        if (!isOpen && CursorManager.Instance.GetUIOpenCount() >= 1) return;

        isOpen = !isOpen; // Toggle state

        foreach (GameObject panel in backpackUI)
        {
            if (panel != null)
            {
                panel.SetActive(isOpen);
            }
        }

        if (isOpen)
        {
            CursorManager.Instance.UIOpened(); // Notify UI Manager when opened
        }
        else
        {
            CursorManager.Instance.UIClosed(); // Notify UI Manager when closed
        }

        NewUIManager.Instance.RenderInventory();
        UpdateBagIcon(); // Update bag image
    }


    private void UpdateBagIcon()
    {
        if (bagIcon != null && bagSprites.Length >= 2)
        {
            bagIcon.sprite = isOpen ? bagSprites[1] : bagSprites[0]; // Change icon
        }
    }

    public bool IsBackpackOpen()
    {
        return isOpen;
    }
}
