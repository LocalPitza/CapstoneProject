using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Bedroom, PlantingArea, TestCity}
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
        SceneManager.LoadScene(locationToSwitch.ToString());
    }

    //Called when a scene is loaded
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        //The location the player is coming from when the scene loads
        Location oldLocation = currentLocation;

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
