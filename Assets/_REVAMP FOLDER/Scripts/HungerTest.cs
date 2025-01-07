using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerTest : MonoBehaviour
{
    [SerializeField] private float hungerDecreaseInterval = 1f;
    [SerializeField] private int hungerDecreaseAmount = 1;

    private float hungerTimer;

    private void Update()
    {
        hungerTimer += Time.deltaTime;
        if (hungerTimer >= hungerDecreaseInterval)
        {
            PlayerStats.HungerStat(-hungerDecreaseAmount);
            hungerTimer = 0;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            PlayerStats.HungerStat(10);
        }
    }
}
