using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public bool CanMove { get; set; } = true;


    private CharacterController controller;

    public float speed = 5f;
    public GameObject playerObject;
    public float turnSpeed = 180f;
    public Vector2 CurrentInput;

    PlayerInteraction playerInteraction;

    public static bool isUIOpen = false;


    [SerializeField] private float baseStepSpeed = 0.1f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] stoneClips = default;
    [SerializeField] private AudioClip[] tileClips = default;
    [SerializeField] private AudioClip[] metalClips = default;
    [SerializeField] private float footstepTimer = 0;
    private float GetCurrentOffset => baseStepSpeed;


    private void Start()
    {
        controller = GetComponent<CharacterController>();
        playerInteraction = GetComponentInChildren<PlayerInteraction>();
    }

    private void Update()
    {
        if(CanMove)
        {
            HandleFootstep();
            Interact();
            HandleMovement();
        }

        if (isUIOpen) return;

    }

    public void Interact()
    {
        // Use InputManager to check for key presses
        if (Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            playerInteraction.Interact();
        }

        if (Input.GetKeyDown(InputManager.Instance.harvestKey))
        {
            playerInteraction.HarvestInteract();
        }

        if (Input.GetKeyDown(InputManager.Instance.harvestKeepKey))
        {
            playerInteraction.HarvestKeep();
        }
    }

    private void HandleMovement()
    {
        Vector3 movDir;

        CurrentInput = new Vector2((speed) * Input.GetAxis("Vertical"), (speed) * Input.GetAxis("Horizontal"));

        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime, 0);
        movDir = transform.forward * Input.GetAxis("Vertical") * speed;

        controller.Move(movDir * Time.deltaTime - Vector3.up * 0.1f);
    }

    private void HandleFootstep()
    {
        if (!controller.isGrounded) return;
        if (CurrentInput == Vector2.zero) return;

        footstepTimer -= Time.deltaTime;

        if(footstepTimer <= 0)
        {
            if(Physics.Raycast(playerObject.transform.position, Vector3.down, out RaycastHit hit, 30 , LayerMask.GetMask("Floor")))
            {
                switch (hit.collider.tag)
                {
                    case "Footstep/WOOD":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footstep/STONE":
                        footstepAudioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                    case "Footstep/TILE":
                        footstepAudioSource.PlayOneShot(tileClips[Random.Range(0, tileClips.Length - 1)]);
                        break;
                    case "Footstep/METAL":
                        footstepAudioSource.PlayOneShot(metalClips[Random.Range(0, metalClips.Length - 1)]);
                        break;
                    default:
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }
}
