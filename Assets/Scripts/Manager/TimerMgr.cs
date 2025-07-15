using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerMgr : MonoBehaviour
{
    [SerializeField] private float timer;
    private bool isRunning;

    [SerializeField] private GameMgr _gameMgr;
    [SerializeField] private UIManager _UIMger;

    public void StartTimer(float time)
    {
        timer = time;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public float GetTime() => timer;

    private void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0;
            isRunning = false;
            _gameMgr.GameOver();
        }
        _UIMger.UpdateTimerUI(timer);
    }
}
