using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    // Key bindings (defaults)
    public KeyCode interactKey = KeyCode.F;
    public KeyCode harvestKey = KeyCode.E;
    public KeyCode harvestKeepKey = KeyCode.Q;
    public KeyCode openJournal = KeyCode.O;
    public KeyCode openBackpack = KeyCode.I;

    private void Awake()
    {
        // Ensure a single instance of InputManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
