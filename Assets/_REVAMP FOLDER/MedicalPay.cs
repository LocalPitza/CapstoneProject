using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    

    public void PayMedical(string cause)
    {
        Debug.Log($"Game Over! Cause: {cause}");

        PlayerMove.isUIOpen = true;

        // Show the correct cause on the UI
        causeText.text = "Cause of medical emergency: " + cause;

        // Activate the Medical UI
        medicalPayUI.SetActive(true);

        PlayerStats.Spend(medicalCost, "Failed to pay medical expenses");
    }

    public void CloseMedicalUI()
    {
        medicalPayUI.SetActive(false);

        PlayerMove.isUIOpen = false;
    }
}
