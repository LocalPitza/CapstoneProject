using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI interactionText;

    private void Start()
    {
        InteractMessage.message = interactionText;
        PlayerInteraction.message = interactionText;
    }
}
