using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// TimerBehaviour class to use a Timer with Unity implementation.
/// /// Author(s): Jason Cheung
/// Date: Oct 21 2022
/// Source(s):
///     Game Architecture Tips - Unity: https://youtu.be/pRjTM3pzqDw
/// Remarks: If creating a Unity Component for a Timer - use this script.
/// Change History:
/// </summary>
public class TimerBehaviour : MonoBehaviour
{
    // duration of Timer; default is 3 seconds.
    [SerializeField] private float duration = 3f;
    
    // customizable Unity Event when the Timer ends
    [SerializeField] private UnityEvent onTimerEnd = null;

    // Timer Object
    private Timer timer;

    // Start is called before the first frame update
    private void Start()
    {
        timer = new Timer(duration);

        timer.OnTimerEnd += HandleTimerEnd;
    }

    // The Invoked Unity Event when the Timer ends
    private void HandleTimerEnd()
    {
        // raise the Unity Event
        onTimerEnd.Invoke();

        // destroys this component but not the GameObject
        Destroy(this);
    }

    // Update is called once per frame
    private void Update()
    {
        // Tick the timer every frame
        timer.Tick(Time.deltaTime);
    }
}
