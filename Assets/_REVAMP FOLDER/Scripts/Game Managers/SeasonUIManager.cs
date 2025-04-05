using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeasonUIManager : MonoBehaviour, ITimeTracker
{
    public Image seasonIcon;

    [Header("Season Icons")]
    public Sprite rainyIcon;
    public Sprite coolDryIcon;
    public Sprite hotDryIcon;

    private void Start()
    {
        // Register with TimeManager to get updates
        TimeManager.Instance.RegisterTracker(this);

        // Set the current icon based on initial time
        ClockUpdate(TimeManager.Instance.GetGameTimeStamp());
    }

    public void ClockUpdate(GameTimeStamp timeStamp)
    {
        switch (timeStamp.season)
        {
            case GameTimeStamp.Season.Rainy:
                seasonIcon.sprite = rainyIcon;
                break;
            case GameTimeStamp.Season.CoolDry:
                seasonIcon.sprite = coolDryIcon;
                break;
            case GameTimeStamp.Season.HotDry:
                seasonIcon.sprite = hotDryIcon;
                break;
        }
    }

    private void OnDestroy()
    {
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.UnregisterTracker(this);
        }
    }
}
