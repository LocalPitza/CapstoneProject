using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public bool CanMove { get; set; } = true;


    private CharacterController controller;
    public Animator animator;

    public float speed = 5f;
    public float runSpeed = 8f;
    private float currentSpeed;
    public GameObject playerObject;
    public float turnSpeed = 180f;
    public Vector2 CurrentInput;

    public float mouseSensitivity = 5f;
    private float rotationX = 0f;

    PlayerInteraction playerInteraction;

    public static bool isUIOpen = false;
    public static bool isInTeleportTrigger = false;

    public bool useMouseLook = true;
    public float keyboardTurnSpeed = 90f;

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
        currentSpeed = speed;

        useMouseLook = PlayerPrefs.GetInt("UseMouseLook", 1) == 1;
        mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 5f);
    }

    private void Update()
    {
        if (isUIOpen) return;

        HandleMouseLock();

        if (CanMove)
        {
            HandleRotation(); // Updated function to support both rotation methods
            HandleFootstep();
            if (!isInTeleportTrigger)
            {
                Interact();
            }
            HandleMovement();

            animator.SetBool("IsWalking", Input.GetButton("Vertical"));
        }

        //Debug.LogWarning("Player is in Teleport Trigger:" + isInTeleportTrigger);

    }

    public void Interact()
    {
        // Use InputManager to check for key presses
        if (Input.GetKeyDown(InputManager.Instance.interactKey))
        {
            if (playerInteraction.HasHarvestable())
            {
                playerInteraction.HarvestInteract();
            }
            else
            {
                playerInteraction.Interact();
            }
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

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = speed;
        }
        
        Vector3 movDir;
        CurrentInput = new Vector2((currentSpeed) * Input.GetAxis("Vertical"), (currentSpeed) * Input.GetAxis("Horizontal"));

        if (!Input.GetKey(KeyCode.LeftAlt)) // Prevent rotation while LeftAlt is held
        {
            movDir = transform.forward * Input.GetAxis("Vertical") * currentSpeed;
            controller.Move(movDir * Time.deltaTime - Vector3.up * 0.1f);
        }
    }

    private void HandleRotation()
    {
        if (useMouseLook)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            Quaternion newRotation = Quaternion.Euler(0, mouseX, 0);
            transform.rotation = Quaternion.Lerp(transform.rotation, transform.rotation * newRotation, Time.deltaTime * turnSpeed);
        }
        else
        {
            float rotateInput = 0f;
            if (Input.GetKey(KeyCode.A))
            {
                rotateInput = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                rotateInput = 1f;
            }

            transform.Rotate(Vector3.up * rotateInput * keyboardTurnSpeed * Time.deltaTime);
        }
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
    public void SetUseMouseLook(bool value)
    {
        useMouseLook = value;
        PlayerPrefs.SetInt("UseMouseLook", value ? 1 : 0);
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
        PlayerPrefs.SetFloat("MouseSensitivity", value);
    }
}
