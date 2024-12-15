using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance { get; private set; }

    public List<StartPoint> startPoints;

    public void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public Transform GetPlayerStartingPosition(SceneTransitionManager.Location enteringFrom)
    {
        StartPoint startingPoint = startPoints.Find(x => x.enteringFrom == enteringFrom);
        return startingPoint.playerStart;
    }
}
