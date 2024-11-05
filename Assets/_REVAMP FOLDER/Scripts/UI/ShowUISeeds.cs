using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUISeeds : MonoBehaviour
{
    [SerializeField] GameObject plantingSeedsUI;

    void Start()
    {
        plantingSeedsUI.SetActive(false);
    }

    public void ToggleUI()
    {
        plantingSeedsUI.SetActive(!plantingSeedsUI.activeSelf);
    }

    public bool IsUIActive()
    {
        return plantingSeedsUI.activeSelf;
    }
}
