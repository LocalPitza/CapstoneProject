using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [Header("Internal Clock")]
    [SerializeField] GameTimeStamp gameTimeStamp;
    public float timeScale = 1.0f;

    // Added property to get the current day easily
    public int CurrentDay => gameTimeStamp.day;

    [Header("Sun")]
    public Transform sunTransform;

    //List of Objects to Inform of chnages to the time
    List<ITimeTracker> listeners = new List<ITimeTracker>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        gameTimeStamp = new GameTimeStamp(0 , GameTimeStamp.Season.Rainy, 1, 6, 0);
        StartCoroutine(TimeUpdate());
    }

    public void LoadTime(GameTimeStamp timeStamp)
    {
        this.gameTimeStamp = new GameTimeStamp(timeStamp);
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            Tick();
            yield return new WaitForSeconds(1/timeScale);
        }
    }

    public void Tick()
    {
        gameTimeStamp.UpdateClock();

        foreach(ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(gameTimeStamp);
        }

        UpdateSunMovment();
    }

    public void SkipTime(GameTimeStamp timeSkipTo)
    {
        //Skip Time
        int timeToSkipInMinutes = GameTimeStamp.TimestampInMinutes(timeSkipTo);
        Debug.Log("Time to skip to" + timeToSkipInMinutes);

        int timeNowInMinutes = GameTimeStamp.TimestampInMinutes(gameTimeStamp);
        Debug.Log("Time now" + timeNowInMinutes);

        int differentInMinutes = timeToSkipInMinutes - timeNowInMinutes;
        Debug.Log(differentInMinutes + "minutes will be advanced");

        if (differentInMinutes <= 0) return;

        for(int i= 0 ; i < differentInMinutes; i++)
        {
            Tick();
        }
    }

    //Day and Night Cycle
    void UpdateSunMovment()
    {
        int timeInMinutes = GameTimeStamp.HoursToMinutes(gameTimeStamp.hour) + gameTimeStamp.minute;

        float sunAngle = .25f * timeInMinutes - 90;

        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public GameTimeStamp GetGameTimeStamp()
    {
        return new GameTimeStamp(gameTimeStamp);
    }

    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }

    public void UnregisterTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }
}
