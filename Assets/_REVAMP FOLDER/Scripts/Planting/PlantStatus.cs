using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlantStatus : MonoBehaviour
{
    public static PlantStatus Instance { get; private set; }

    [Header("Guide Section")]
    public GameObject guideBg;
    public TMP_Text guideText;

    [Header("Plant Info Section")]
    public GameObject plantInfoBg;
    public TMP_Text plantInformtation;

    [Header("Parent Informations")]
    public GameObject statusPanel;

    private void Awake()
    {
        // Singleton Pattern Setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        HideStatus();
    }

    public void ShowStatus(string guide, string plantInfo)
    {
        bool showAny = false;

        // Guide Section
        if (guideText != null)
        {
            guideText.text = guide;
            if (guideBg != null)
                guideBg.SetActive(!string.IsNullOrEmpty(guide));

            if (!string.IsNullOrEmpty(guide))
                showAny = true;
        }

        // Plant Info Section
        if (plantInformtation != null)
        {
            plantInformtation.text = plantInfo;
            if (plantInfoBg != null)
                plantInfoBg.SetActive(!string.IsNullOrEmpty(plantInfo));

            if (!string.IsNullOrEmpty(plantInfo))
                showAny = true;
        }

        // Only show the whole status panel if either section is not empty
        if (statusPanel != null)
        {
            statusPanel.SetActive(showAny);
        }
    }

    public void HideStatus()
    {
        if (guideText != null) guideText.text = "";
        if (plantInformtation != null) plantInformtation.text = "";

        if (guideBg != null) guideBg.SetActive(false);
        if (plantInfoBg != null) plantInfoBg.SetActive(false);

        if (statusPanel != null) statusPanel.SetActive(false);
    }
}
