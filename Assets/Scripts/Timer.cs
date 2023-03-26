using UnityEngine;

public class Timer
{
    private float timer;
    private float timeSeconds;

    public Timer( float timeSeconds ) 
    {
        SetNewTime( timeSeconds );
    }

    public float SecondsLeft()
    {
        return timer;
    }

    public float SecondsElapsed()
    {
        return timeSeconds - timer;
    }

    public bool HasTimeUp()
    {
        timer -= Time.deltaTime;
        if ( timer <= 0 )
        {
            timer += timeSeconds;
            return true;
        }
        return false;
    }

    public void SetNewTime( float timeSeconds )
    {
        timer = timeSeconds;
        this.timeSeconds = timeSeconds;
    }
}
