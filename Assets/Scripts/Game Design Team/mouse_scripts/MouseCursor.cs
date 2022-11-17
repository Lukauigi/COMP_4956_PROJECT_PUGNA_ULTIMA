using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{

    private TrailRenderer trail;
    public float timeBetweenSpawn = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        trail = GetComponent<TrailRenderer>();
    }

    //Trail follows mouse cursor position.
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

    }
}
