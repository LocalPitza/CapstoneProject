using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenBackpack : MonoBehaviour
{
    public GameObject[] backpackUI;

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
        bool isActive = false;

        foreach (GameObject panel in backpackUI)
        {
            if (panel != null)
            {
                panel.SetActive(!panel.activeSelf);
                isActive = panel.activeSelf; // Determine the final state
            }
        }

        PlayerMove.isUIOpen = isActive;

        NewUIManager.Instance.RenderInventory();

        UpdateCursorState(isActive);
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
}
