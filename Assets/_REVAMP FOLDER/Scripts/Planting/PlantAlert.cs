using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAlert : MonoBehaviour
{
    public float floatSpeed = 1f;       // Speed of the up-and-down movement
    public float floatAmount = 0.5f;    // How high/low it moves

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        // PingPong returns a value that oscillates between 0 and floatAmount
        float newY = Mathf.PingPong(Time.time * floatSpeed, floatAmount);
        transform.localPosition = startPos + new Vector3(0, newY, 0);
    }
}
