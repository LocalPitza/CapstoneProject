using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.ProBuilder.Shapes;

public class Teleport : MonoBehaviour
{
    [Header("Teleport Settings")]

    // The destination Transform to teleport to
    public Transform teleportDestination;
    public GameObject guideUI;
    public CinemachineVirtualCamera targetCamera;
    //[SerializeField] float fadeDuration = 0.5f;

    // The tag to identify the player
    public string playerTag = "Player";
    public string DoorAudio;
    public float teleportSpeed;

    private GameObject playerInTrigger;

    private void Start()
    {
        if(guideUI != null)
        {
            guideUI.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = other.gameObject;
            PlayerMove.isInTeleportTrigger = true; //Prevent interaction
            if (guideUI != null)
            {
                guideUI.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Clear the reference when the player leaves the trigger
        if (other.CompareTag(playerTag))
        {
            playerInTrigger = null;
            PlayerMove.isInTeleportTrigger = false; //Allow interaction again
            if (guideUI != null)
            {
                guideUI.SetActive(false);
            }
        }
    }

    void Update()
    {
        if (guideUI != null)
        {
            guideUI.SetActive(playerInTrigger);

            if (targetCamera != null)
            {
                Vector3 direction = guideUI.transform.position - targetCamera.transform.position;
                guideUI.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            }
        }

        // Check if the player is in the trigger and the F key is pressed
        if (playerInTrigger != null && Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            FindObjectOfType<SoundManager>().Play(DoorAudio);
            //FadeManager.Instance.SetFadeDuration(fadeDuration);
            //FadeManager.Instance.FadeIn();
            StartCoroutine(TeleportAfterFade());
        }
    }  

    private IEnumerator TeleportAfterFade()
    {
        PlayerMove.isUIOpen = true;

        // Start fade out
        yield return GameStateManager.Instance.FadeOut();

        // Teleport the player
        TeleportPlayer();

        // Start fade in
        yield return GameStateManager.Instance.FadeIn();

        PlayerMove.isUIOpen = false;

        /*PlayerMove.isUIOpen = true;
        TeleportPlayer();
        yield return new WaitForSeconds(fadeDuration);
        TeleportPlayer();
        yield return new WaitForSeconds(fadeDuration);
        FadeManager.Instance.FadeOut();
        PlayerMove.isUIOpen = false;*/
    }

    private void TeleportPlayer()
    {
        //FindObjectOfType<SoundManager>().Play(DoorAudio);
        //playerInTrigger.transform.DOMove(teleportDestination, teleportSpeed);
        playerInTrigger.transform.position = teleportDestination.position;
        playerInTrigger.transform.rotation = teleportDestination.rotation;
        Debug.Log("Player teleported to " + teleportDestination);
    }
}
