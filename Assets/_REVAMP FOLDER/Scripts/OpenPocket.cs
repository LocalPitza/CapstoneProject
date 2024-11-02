using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenPocket : MonoBehaviour
{
    public GameObject pocketPanel;

    private void Start()
    {
        pocketPanel.SetActive(false);
    }

    public void TogglePocketPanel()
    {
        pocketPanel.SetActive(!pocketPanel.activeSelf);
        NewUIManager.Instance.RenderInventory();
    }
}
