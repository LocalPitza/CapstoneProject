using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Start()
    {
        GameObject essentials = GameObject.Find("Essentials");
        if (essentials != null)
        {
            Transform textBoxTransform = essentials.transform.Find("PlayerCanvas/InteractText/playerTextBox");

            if (textBoxTransform != null)
            {
                interactionText = textBoxTransform.GetComponent<TextMeshProUGUI>();
                if (interactionText == null)
                {
                    Debug.LogError("TextMeshProUGUI component not found on playerTextBox.");
                }
            }
            else
            {
                Debug.LogError("interactionTextBox not found in the specified path.");
            }
        }
        else
        {
            Debug.LogError("Essentials prefab not found in the scene.");
        }
    }

    public TextMeshProUGUI GetInteractionText()
    {
        return interactionText;
    }
}
