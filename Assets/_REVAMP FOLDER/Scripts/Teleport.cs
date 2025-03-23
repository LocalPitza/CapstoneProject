using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Video;

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

    [Header("Video Settings")]
    public bool playCutscene = false; // Enable/Disable cutscene
    public VideoPlayer videoPlayer; // Assign in Inspector
    public GameObject videoScreen; // UI or World Space Screen for the video

    private GameObject playerInTrigger;

    private void Start()
    {
        if(guideUI != null)
        {
            guideUI.SetActive(false);
        }

        videoScreen.gameObject.SetActive(false);
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

        // Prevent teleportation if the screen is fading
        if (playerInTrigger != null && Input.GetKeyDown(InputManager.Instance.interactKey) && !GameStateManager.Instance.IsFading)
        {
            FindObjectOfType<SoundManager>().Play(DoorAudio);
            StartCoroutine(TeleportAfterFade());
        }
    }  

    private IEnumerator TeleportAfterFade()
    {
        PlayerMove.isUIOpen = true;

        // Start fade out
        yield return GameStateManager.Instance.FadeOut();

        if (playCutscene && videoPlayer != null && videoScreen != null)
        {
            videoScreen.SetActive(true);
            videoPlayer.Stop();
            videoPlayer.time = 0;
            Debug.Log($"Playing video: {videoPlayer.clip.name}");
            videoPlayer.Play();

            while (videoPlayer.isPlaying)
            {
                yield return null;
            }
            videoScreen.SetActive(false);
        }

        // Teleport the player
        TeleportPlayer();

        // Wait for 1 second while the screen stays black
        yield return new WaitForSeconds(1.5f);

        // Start fade in
        yield return GameStateManager.Instance.FadeIn();

        PlayerMove.isUIOpen = false;
    }

    private void TeleportPlayer()
    {
        playerInTrigger.transform.position = teleportDestination.position;
        playerInTrigger.transform.rotation = teleportDestination.rotation;
        Debug.Log("Player teleported to " + teleportDestination);
    }
}
