using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageInteract : MonoBehaviour
{
    [SerializeField] GameObject storageUI;
    private InteractMessage interactMessage;

    void Start()
    {
        storageUI.SetActive(false);
        interactMessage = GetComponent<InteractMessage>();
    }

    private void Update()
    {
        if (interactMessage != null && interactMessage.IsPlayerInRange() && Input.GetKeyDown(KeyCode.F))
        {
            storageUI.SetActive(!storageUI.activeSelf);
            NewUIManager.Instance.RenderInventory();
        }
    }

    public bool IsUIActive()
    {
        return storageUI.activeSelf;
    }
}
