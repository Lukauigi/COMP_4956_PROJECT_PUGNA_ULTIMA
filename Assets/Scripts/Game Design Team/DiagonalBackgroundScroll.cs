using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scrolling background script for diagonal scroll.
/// Authors: Gabriel Baluyut
/// Date: October 27, 2022
/// Sources:
/// Scrolling Background in 90 seconds - Unity Tutorial: https://www.youtube.com/watch?v=-6H-uYh80vc
/// Revised: 
/// October 29 - Moved horizontal scrolling to separate script so it can be added easily.
/// </summary>
public class DiagonalBackgroundScroll : MonoBehaviour
{
    [SerializeField]
    private RawImage _backgroundImage;

    [SerializeField]
    private float _xCoordinate, _yCoordinate;

    // Updates position of image using a RawImage to set the x and y values dynamically.
    void Update()
    {
        // To have backgrounds scrolling diagonally (update x and y)
        _backgroundImage.uvRect = new Rect(_backgroundImage.uvRect.position + new Vector2(_xCoordinate, _yCoordinate) * Time.deltaTime, _backgroundImage.uvRect.size);
    }
}
