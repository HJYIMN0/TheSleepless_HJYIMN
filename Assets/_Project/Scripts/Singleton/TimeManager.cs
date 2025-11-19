using System;
using System.Collections;
using UnityEngine;

public class TimeManager : GenericSingleton<TimeManager>
{
    public override bool ShouldDetatchFromParent() => true;
    public override bool IsDestroyedOnLoad() => false;

    [SerializeField] private float standardTimeScale = 1f;
    [SerializeField] private float secondDuration = 2f;
    [SerializeField] private int minutesPassingPerSecond = 1;

    private TimeState currentTimeState = TimeState.Running;
    public TimeState CurrentTimeState => currentTimeState;


    public float StandardTimeScale => standardTimeScale;
    private GameManager _gameManagerInstance => GameManager.Instance;

    public Action<float> OnTimeChanged;

    private int hour;
    private int minute;
    private int day;

    public float StartandardTimeScale => standardTimeScale;

    public void SetCurrentTimeState(TimeState newState)
    {
        if (currentTimeState == newState) return;
        currentTimeState = newState;
        StartCoroutine(StartTime());
    }
    
    private void Start()
    {
        hour = _gameManagerInstance.Hour;
        minute = _gameManagerInstance.Minute;
        day = _gameManagerInstance.Day;

        SetCurrentTimeState(TimeState.Running);
        StartCoroutine(StartTime());
    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.Escape))
    //    {
    //        SetCurrentTimeState(TimeState.Paused);
    //    }
    //}

    public IEnumerator StartTime()
    {
        while (currentTimeState == TimeState.Running)
        {
            _gameManagerInstance.IncreaseHour(minutesPassingPerSecond);
            yield return new WaitForSeconds(secondDuration);
        }
    }
}
