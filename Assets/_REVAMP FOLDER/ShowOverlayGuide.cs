using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowOverlayGuide : MonoBehaviour
{
    public GameObject overlayGuide;
    public TextMeshProUGUI toggleButtonText;

    void Start()
    {
        // Ensure the overlay is active at start
        if (overlayGuide != null)
            overlayGuide.SetActive(true);

        UpdateButtonText();
    }

    void Update()
    {
        // Toggle overlay with F1 key
        if (Input.GetKeyDown(KeyCode.F1))
        {
            ToggleOverlay();
        }
    }

    // Function for button press
    public void ToggleOverlay()
    {
        if (overlayGuide != null)
        {
            overlayGuide.SetActive(!overlayGuide.activeSelf);
            UpdateButtonText();
        }
    }

    // Update the button text based on overlay state
    private void UpdateButtonText()
    {
        if (toggleButtonText != null)
        {
            toggleButtonText.text = overlayGuide.activeSelf ? "Hide Overlay" : "Show Overlay";
        }
    }
}
