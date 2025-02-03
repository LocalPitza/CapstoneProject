using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeShop : MonoBehaviour
{
    [SerializeField] GameObject upgradeStoreUI;

    private InteractMessage interactMessage;

    void Start()
    {
        interactMessage = GetComponent<InteractMessage>();
    }

    void Update()
    {
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            upgradeStoreUI.SetActive(true);
            PlayerMove.isUIOpen = true;
        }
    }

    public void CloseUpgradeShop()
    {
        upgradeStoreUI.SetActive(false);
        PlayerMove.isUIOpen = false;
    }
}
