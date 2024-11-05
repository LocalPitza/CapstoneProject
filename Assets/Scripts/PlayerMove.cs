using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private CharacterController controller;

    public float speed = 5f;
    public float turnSpeed = 180f;

    PlayerInteraction playerInteraction;
    private ShowUISeeds showUISeeds;
    private StorageInteract storageInteract;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInteraction = GetComponentInChildren<PlayerInteraction>();

        storageInteract = FindObjectOfType<StorageInteract>();
    }

    private void Update()
    {
        if ((showUISeeds == null || !showUISeeds.IsUIActive()) &&
            (storageInteract == null || !storageInteract.IsUIActive()))
        {
            Vector3 movDir;

            transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
            movDir = transform.forward * Input.GetAxis("Vertical") * speed;

            controller.Move(movDir * Time.deltaTime - Vector3.up * 0.1f);
        }

        Interact();
    }

    public void Interact()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (playerInteraction.harvestableHit)
            {
                Debug.Log("Harvesting");
                playerInteraction.HarvestInteract();
            }
            else
            {
                playerInteraction.Interact();
            }

            if (playerInteraction.selectedSoil != null)
            {
                showUISeeds = playerInteraction.selectedSoil.GetComponent<ShowUISeeds>();
            }
        }
    }
}
