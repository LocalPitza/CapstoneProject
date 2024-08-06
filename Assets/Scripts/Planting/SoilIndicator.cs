using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoilIndicator : MonoBehaviour
{
    new Renderer renderer;

    public GameObject indicator;

    public void Select(bool toggle)
    {
        indicator.SetActive(toggle);
    }
}
