using System;

/// <summary>
/// Timer object with pure C# code. If creating a Unity Component for Timer - use TimerBehaviour script.
/// Author(s): Jason Cheung
/// Date: Oct 21 2022
/// Source(s):
///     Game Architecture Tips - Unity: https://youtu.be/pRjTM3pzqDw
/// Remarks: Contains only C# logic. Try to keep Unity implementation in TimerBehaviour.
/// Change History:
/// </summary>
public class Timer
{

    public float RemainingSeconds { get; private set; }

    public event Action OnTimerEnd;

    public Timer(float duration)
    {
        RemainingSeconds = duration;
    }

    public void Tick(float deltaTime)
    {
        // stop timer if 0
        if (RemainingSeconds == 0f) { return; }

        RemainingSeconds -= deltaTime;

        CheckForTimerEnd();
    }

    private void CheckForTimerEnd()
    {
        // return if timer is still ongoing
        if (RemainingSeconds > 0f) { return; }

        // prevent negative seconds
        RemainingSeconds = 0f;

        OnTimerEnd?.Invoke();
    }
}
