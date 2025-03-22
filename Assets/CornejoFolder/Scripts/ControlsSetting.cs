using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsSetting : MonoBehaviour
{
    [Header("Mouse Look Settings")]
    public Toggle mouseLookToggle;
    public Slider sensitivitySlider;
    public TMP_Text sensitivityValueText;

    [Header("Key Bindings")]
    public Button interactButton;
    public Button harvestButton;
    public Button journalButton;
    public Button backpackButton;
    public TMP_Text interactText;
    public TMP_Text harvestText;
    public TMP_Text journalText;
    public TMP_Text backpackText;

    private string waitingForKey = null;
    private const float MAX_SENSITIVITY = 10f;

    private void Start()
    {
        // Load Mouse Look settings
        bool useMouseLook = PlayerPrefs.GetInt("UseMouseLook", 1) == 1;
        float mouseSensitivity = Mathf.Clamp(PlayerPrefs.GetFloat("MouseSensitivity", 5f), 0.1f, MAX_SENSITIVITY);

        // Apply Mouse Look settings
        mouseLookToggle.isOn = useMouseLook;
        sensitivitySlider.maxValue = MAX_SENSITIVITY; // Ensure max is set
        sensitivitySlider.value = mouseSensitivity;
        sensitivityValueText.text = mouseSensitivity.ToString("F1");

        // Add Mouse Look Listeners
        mouseLookToggle.onValueChanged.AddListener(SetMouseLook);
        sensitivitySlider.onValueChanged.AddListener(SetMouseSensitivity);

        // Load Key Bindings
        UpdateKeyTexts();

        // Add Listeners for Key Rebinding
        interactButton.onClick.AddListener(() => StartRebinding("Interact"));
        harvestButton.onClick.AddListener(() => StartRebinding("Harvest"));
        journalButton.onClick.AddListener(() => StartRebinding("Journal"));
        backpackButton.onClick.AddListener(() => StartRebinding("Backpack"));
    }

    private void Update()
    {
        if (waitingForKey != null && Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    InputManager.Instance.SetKeyBinding(waitingForKey, key);
                    waitingForKey = null;
                    UpdateKeyTexts();
                    return;
                }
            }
        }
    }

    private void StartRebinding(string keyName)
    {
        waitingForKey = keyName;
        Debug.Log($"Press any key to set {keyName}");
    }

    private void UpdateKeyTexts()
    {
        interactText.text = InputManager.Instance.interactKey.ToString();
        harvestText.text = InputManager.Instance.harvestKey.ToString();
        journalText.text = InputManager.Instance.openJournal.ToString();
        backpackText.text = InputManager.Instance.openBackpack.ToString();
    }

    public void SetMouseLook(bool isEnabled)
    {
        PlayerPrefs.SetInt("UseMouseLook", isEnabled ? 1 : 0);
        PlayerMove player = FindObjectOfType<PlayerMove>();
        if (player != null)
        {
            player.SetUseMouseLook(isEnabled);
        }
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        sensitivity = Mathf.Clamp(sensitivity, 0.1f, MAX_SENSITIVITY); // Clamp sensitivity
        PlayerPrefs.SetFloat("MouseSensitivity", sensitivity);
        sensitivityValueText.text = sensitivity.ToString("F1");

        PlayerMove player = FindObjectOfType<PlayerMove>();
        if (player != null)
        {
            player.SetMouseSensitivity(sensitivity);
        }
    }
}
