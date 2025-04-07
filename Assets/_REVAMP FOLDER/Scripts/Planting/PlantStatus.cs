using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlantStatus : MonoBehaviour
{
    public static PlantStatus Instance { get; private set; }

    [Header("UI Elements")]
    public GameObject guideBg;
    public TMP_Text guideText;

    private void Awake()
    {
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

    public void ShowStatus(string plantInfo)
    {
        string fullText = "";

        if (!string.IsNullOrEmpty(plantInfo))
            fullText += plantInfo;

        guideText.text = fullText;

        // Just show the background/message
        guideBg.SetActive(true);
    }

    public void HideStatus()
    {
        guideText.text = "";
        guideBg.SetActive(false);
    }
}
