using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scrolling background script for horizontal scroll.
/// Authors: Gabriel Baluyut
/// Date: October 29, 2022
/// Sources:
/// Scrolling Background in 90 seconds - Unity Tutorial: https://www.youtube.com/watch?v=-6H-uYh80vc
/// </summary>
public class HorizontalBackgroundScrolling : MonoBehaviour
{

    [SerializeField]
    private RawImage _backgroundImage;

    [SerializeField]
    private float _xCoordinate, _yCoordinate;

    // Updates position of image using a RawImage to set the x value dynamically.
    void Update()
    {
        _backgroundImage.uvRect = new Rect(_backgroundImage.uvRect.position + new Vector2(_xCoordinate, 0) * Time.deltaTime, _backgroundImage.uvRect.size);
    }
}
