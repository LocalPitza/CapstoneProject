using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MedicalPay : MonoBehaviour
{
    public static MedicalPay Instance;

    [SerializeField] int medicalCost;
    public GameObject medicalPayUI;
    public TextMeshProUGUI causeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

    }

    private void Update()
    {
        // Stop hunger decrease if medicalPrompt is active
        if (medicalPayUI != null && medicalPayUI.activeSelf)
        {
            CursorManager.Instance.UIOpened();
        }
    }

    public void PayMedical(string cause)
    {
        Debug.Log($"Game Over! Cause: {cause}");

        causeText.text = "Cause of medical emergency: " + cause;
        medicalPayUI.SetActive(true);

        PlayerStats.Spend(medicalCost, "Failed to pay medical expenses");
    }

    public void CloseMedicalUI()
    {
        medicalPayUI.SetActive(false);

        CursorManager.Instance.UIClosed();
    }
}
