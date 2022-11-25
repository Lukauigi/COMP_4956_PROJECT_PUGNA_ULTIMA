using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the login screen
/// Authors: John Ryue
/// Date: Nov 10 2022
/// Source:
///     Mouse cursor - https://www.youtube.com/watch?v=cCKlMAwvQcI
///     Mouse trail effect - https://www.youtube.com/watch?v=_TcEfIXpmRI 
/// </summary>
public class MouseCursor : MonoBehaviour
{

    private TrailRenderer trail;
    public float timeBetweenSpawn = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        trail = GetComponent<TrailRenderer>();
    }

    //Trail follows mouse cursor position.
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

    }
}
