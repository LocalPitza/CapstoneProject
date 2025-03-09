using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public static CursorManager Instance;

    private int uiOpenCount = 0; // Track how many UIs are open

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void UIOpened()
    {
        uiOpenCount++;
        UpdateCursorState();
    }

    public void UIClosed()
    {
        uiOpenCount = Mathf.Max(0, uiOpenCount - 1); // Prevent negative values
        UpdateCursorState();
    }

    private void UpdateCursorState()
    {
        if (uiOpenCount > 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            PlayerMove.isUIOpen = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            PlayerMove.isUIOpen = false;
        }
    }
}
