using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct StartPoint
{
    public SceneTransitionManager.Location enteringFrom;

    public Transform playerStart;
}
