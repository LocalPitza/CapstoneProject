using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public Transform player;
    public Light lightSource;
    public float maxDistance = 10f;
    public float maxIntensity = 1.5f;
    public float minIntensity = 0f;
    public float fadeSpeed = 2f;

    private void Start()
    {
        lightSource = GetComponent<Light>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        player = playerObj.transform;
        
        if (lightSource == null)
        {
            Debug.LogError("No Light component found on this GameObject. Please add one.");
        }
    }
    private void Update()
    {
        if (lightSource != null && player != null)
        {
            
            float distanceToPlayer = Vector3.Distance(player.position, lightSource.transform.position);

            if (distanceToPlayer <= maxDistance)
            {
                float intensity = Mathf.Lerp(minIntensity, maxIntensity, 1 - (distanceToPlayer / maxDistance));
                lightSource.intensity = Mathf.Lerp(lightSource.intensity, intensity, Time.deltaTime * fadeSpeed);
            }
            else
            {
                lightSource.intensity = Mathf.Lerp(lightSource.intensity, minIntensity, Time.deltaTime * fadeSpeed);
            }
        }
    }
}
