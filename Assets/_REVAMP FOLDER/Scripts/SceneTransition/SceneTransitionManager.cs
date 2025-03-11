using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Bedroom, PlantingArea, TestCity, MainGame, NewBedroom}
    public Location currentLocation;

    Transform playerPoint;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnLocationLoad;

        playerPoint = FindObjectOfType<PlayerMove>().transform;
    }

    //Switch the player to another scene
    public void SwitchLocation(Location locationToSwitch)
    {
        StartCoroutine(TransitionToLocation(locationToSwitch));
    }

    private IEnumerator TransitionToLocation(Location locationToSwitch)
    {
        PlayerMove.isUIOpen = true;

        // Trigger fade-out
        yield return GameStateManager.Instance.FadeOut();

        // Load the new scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(locationToSwitch.ToString());
        asyncLoad.allowSceneActivation = false;

        // Wait until the scene is almost loaded
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        // Fade-in after the scene is loaded
        yield return GameStateManager.Instance.FadeIn();

        PlayerMove.isUIOpen = false;
    }

    //Called when a scene is loaded
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "NewMenu")
        {
            Debug.Log("Destroying Essential Prefab before loading Main Menu...");

            // Unsubscribe first to prevent further calls
            SceneManager.sceneLoaded -= OnLocationLoad;

            if (gameObject != null)
            {
                // Destroy this object safely
                Destroy(gameObject);
                return;
            }

            return; // Stop execution to prevent further errors
        }

        // Ensure SceneTransitionManager is still valid before accessing anything
        if (this == null) return;

        //The location the player is coming from when the scene loads
        Location oldLocation = currentLocation;

        // Prevent parsing if the scene is not in the enum
        if (!Enum.IsDefined(typeof(Location), scene.name))
        {
            return;
        }

        //Get the new location by converting the string of our current scene into a Location enum value
        Location newLocation = (Location)Enum.Parse(typeof(Location), scene.name);

        //If the player is not coming from any new place, stop executing function
        if (currentLocation == newLocation) return;

        //Find the start point
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

        if (playerPoint == null)
        {
            return;
        }

        //Disable the player's CharacterController component
        CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;

        //Change the player's position to the start point
        playerPoint.position = startPoint.position;
        playerPoint.rotation = startPoint.rotation;

        //Re-enable player character controller so player can move
        playerCharacter.enabled = true;

        //Save the current location that we just switched to
        currentLocation = newLocation;
    }
}
