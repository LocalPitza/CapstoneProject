using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public bool CanMove { get; set; } = true;


    private CharacterController controller;
    public Animator animator;

    public float speed = 5f;
    public GameObject playerObject;
    public float turnSpeed = 180f;
    public Vector2 CurrentInput;

    public float mouseSensitivity = 5f;
    private float rotationX = 0f;

    PlayerInteraction playerInteraction;

    public static bool isUIOpen = false;
    public static bool isInTeleportTrigger = false;

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
        animator = GetComponent<Animator>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (isUIOpen) return;

        HandleMouseLock();

        if (CanMove)
        {
            if (!Input.GetKey(KeyCode.LeftAlt)) // Prevent rotation when LeftAlt is held
            {
                HandleMouseLook();
            }
            HandleFootstep();
            if (!isInTeleportTrigger) //Prevent interaction when in teleport trigger
            {
                Interact();
            }
            HandleMovement();

            if (Input.GetButton("Vertical"))
            {
                animator.SetBool("IsWalking", true);
            }
            else
            {
                animator.SetBool("IsWalking", false);
            }
        }

        //Debug.LogWarning("Player is in Teleport Trigger:" + isInTeleportTrigger);

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

        /*if (Input.GetKeyDown(InputManager.Instance.harvestKeepKey))
        {
            playerInteraction.HarvestKeep();
        }*/
    }

    private void HandleMouseLock()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        Quaternion newRotation = Quaternion.Euler(0, mouseX, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * newRotation, Time.deltaTime * turnSpeed);
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftAlt)) return; 

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
