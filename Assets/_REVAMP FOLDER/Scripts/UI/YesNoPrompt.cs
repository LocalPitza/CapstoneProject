using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class YesNoPrompt : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI promptText;

    Action onYesSelected = null;

    public void CreatePrompt(string message, Action onYesSelected)
    {
        this.onYesSelected = onYesSelected;
        promptText.text = message;
    }

    public void Answer(bool yes)
    {
        if (yes && onYesSelected != null)
        {
            onYesSelected();
        }

        onYesSelected = null;

        gameObject.SetActive(false);
    }
}
