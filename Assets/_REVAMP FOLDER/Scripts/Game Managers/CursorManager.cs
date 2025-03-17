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
        else
        {
            Destroy(gameObject); // Prevent multiple CursorManagers
            return;
        }
    }

    public void UIOpened()
    {
        uiOpenCount++;
        UpdateCursorState();
    }

    public void UIClosed()
    {
        Debug.Log("Cursor Hide, Player Move");
        if (uiOpenCount > 0)
        {
            uiOpenCount--;
        }

        Debug.Log("UI Closed - Count: " + uiOpenCount);

        // Ensure cursor state updates correctly when all UIs close
        UpdateCursorState();
    }

    public void ResetUICount()
    {
        uiOpenCount = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        PlayerMove.isUIOpen = false;
        Debug.Log("UI Count Reset");
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

    public int GetUIOpenCount()
    {
        return uiOpenCount;
    }
}
