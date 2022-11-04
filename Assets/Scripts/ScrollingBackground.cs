using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scrolling background script for controlling movement and update rate.
/// Authors: Gabriel Baluyut
/// Date: October 26, 2022
/// Sources:
/// Unity 2D - Scrolling Background: https://www.youtube.com/watch?v=Wz3nbQPYwss
/// </summary>
public class ScrollingBackground : MonoBehaviour
{

    public float Speed;

    [SerializeField]
    private Renderer BgRenderer;

    // Runs every second to update position of background for movement.
    void Update()
    {
        BgRenderer.material.mainTextureOffset += new Vector2(Speed * Time.deltaTime, 0);
    }
}
