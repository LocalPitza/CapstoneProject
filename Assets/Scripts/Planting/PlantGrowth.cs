using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlantGrowth : MonoBehaviour
{
    [SerializeField] private List<GameObject> plantStages;
    [SerializeField] private List<GameObject> fruits;
    [SerializeField] private float timeBetweenGrowths;
    [SerializeField] private float maxXScale;
    [SerializeField] private float maxYScale;
    [SerializeField] private float maxZScale;
    [SerializeField] private float growthSpeed;
    [SerializeField] private int maxGrowth;
    float startingFruitScale = 0.0f;
    int currentProgress = 0;

    private List<float> fruitScales = new List<float>();
    private bool isGrowing = false;
    private float growthStartTime = 0.0f;

    private List<float> xScales = new List<float>();
    private List<float> yScales = new List<float>();
    private List<float> zScales = new List<float>();

    void Start()
    {
        xScales = new List<float>(fruits.Count);
        yScales = new List<float>(fruits.Count);
        zScales = new List<float>(fruits.Count);

        for (int i = 0; i < fruits.Count; i++)
        {
            xScales.Add(startingFruitScale); // Initialize X-axis scales
            yScales.Add(startingFruitScale); // Initialize Y-axis scales
            zScales.Add(startingFruitScale); // Initialize Z-axis scales
        }
    }

    void Update()
    {
        if (isGrowing)
        {
            float timeElapsed = Time.time - growthStartTime;
            if (timeElapsed >= timeBetweenGrowths)
            {
                // Update current progress and activate/deactivate plant stages
                UpdatePlantStage();
                growthStartTime = Time.time; // Reset timer for next growth stage
            }
        }
    }

    void OnMouseDown()
    {
        if (!isGrowing) // Only start growth if not already growing
        {
            isGrowing = true;
            growthStartTime = Time.time; // Start the growth timer
        }
    }

    public void StartGrowth()
    {
        isGrowing = true;
        growthStartTime = Time.time;
    }

    private void UpdatePlantStage()
    {
        //int currentProgress = 0; //Mathf.Clamp(Mathf.FloorToInt((Time.time - growthStartTime) / timeBetweenGrowths), 0, plantStages.Count - 1);

        for (int i = 0; i < plantStages.Count; i++)
        {
            plantStages[i].SetActive(i == currentProgress);
        }

        if (currentProgress != maxGrowth)
        {
            gameObject.transform.GetChild(currentProgress).gameObject.SetActive(true);
        }

        if (currentProgress > 0 && currentProgress < maxGrowth)
        {
            gameObject.transform.GetChild(currentProgress - 1).gameObject.SetActive(false);
        }

        if (currentProgress < maxGrowth)
        {
            currentProgress++;
        }

        for (int i = 0; i < fruits.Count; i++)
        {
            if (currentProgress == 2) // Adjust this value to trigger growth when desired
            {
                // Calculate growth amount using Mathf.Max for each axis
                float xscaleGrowth = Mathf.Max(xScales[i], maxXScale) - xScales[i];
                float yscaleGrowth = Mathf.Max(yScales[i], maxYScale) - yScales[i];
                float zscaleGrowth = Mathf.Max(zScales[i], maxZScale) - zScales[i];

                // Apply limited growth amount to each axis using separate scales
                xScales[i] += Mathf.Min(xscaleGrowth, growthSpeed * Time.deltaTime);
                yScales[i] += Mathf.Min(yscaleGrowth, growthSpeed * Time.deltaTime);
                zScales[i] += Mathf.Min(zscaleGrowth, growthSpeed * Time.deltaTime);

                // Apply the calculated scales to the fruit object
                fruits[i].transform.localScale = new Vector3(xScales[i], yScales[i], zScales[i]);
            }
        }
    }
}
