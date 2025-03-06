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

        UpdateCursorState(false);
        UpdateBagIcon(); // Set initial icon
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            ToggleBackpackPanel();
        }
    }

    public void ToggleBackpackPanel()
    {
        isOpen = !isOpen; // Toggle state

        foreach (GameObject panel in backpackUI)
        {
            if (panel != null)
            {
                panel.SetActive(isOpen);
            }
        }

        PlayerMove.isUIOpen = isOpen;
        NewUIManager.Instance.RenderInventory();
        UpdateCursorState(isOpen);
        UpdateBagIcon(); // Update bag image
    }

    private void UpdateCursorState(bool isUIOpen)
    {
        if (isUIOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void UpdateBagIcon()
    {
        if (bagIcon != null && bagSprites.Length >= 2)
        {
            bagIcon.sprite = isOpen ? bagSprites[1] : bagSprites[0]; // Change icon
        }
    }
}
