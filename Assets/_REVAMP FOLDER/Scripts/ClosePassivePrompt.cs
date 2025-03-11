using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePassivePrompt : MonoBehaviour
{
    public GameObject medicalPayUI;
    public GameObject rentPayUI;

    public void CloseMedMoney()
    {
        medicalPayUI.SetActive(false);

        CursorManager.Instance.UIClosed();
    }

    public void CloseRentMoney()
    {
        rentPayUI.SetActive(false);

        CursorManager.Instance.UIClosed();
    }
}
