using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Scrolling background script for diagonal top right to bottom left.
/// Authors: Gabriel Baluyut
/// Date: October 27, 2022
/// Sources:
/// Scrolling Background in 90 seconds - Unity Tutorial: https://www.youtube.com/watch?v=-6H-uYh80vc
/// Revised: 
/// October 29 - Added horizontal scrolling code (right to left) for images that need it
/// </summary>
public class DiagonalBackgroundScroll : MonoBehaviour
{
    [SerializeField]
    private RawImage _img;

    [SerializeField]
    private float _x, _y;

    // Updates position of image using a RawImage to set the x and y values dynamically.
    void Update()
    {
        // To have backgrounds scrolling diagonally (update x and y), uncomment the code below
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, _img.uvRect.size);
        // To have backgrounds just scrolling horizontally (right to left), use the code below
        //_img.uvRect = new Rect(_img.uvRect.position + new Vector2(_x, 0) * Time.deltaTime, _img.uvRect.size);
    }
}
