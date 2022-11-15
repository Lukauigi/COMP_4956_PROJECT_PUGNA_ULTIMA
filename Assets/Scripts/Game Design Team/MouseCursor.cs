using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    private SpriteRenderer rend;
    public Sprite pointerCursor;
    public Sprite normalCursor;

    public GameObject clickEffect;
    public GameObject trailEffect;

    public float timeBetweenSpawn = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rend = GetComponent<SpriteRenderer>();
    }

    //When mouse button is held down, changes to a pointer cursor
    //Else, default mouse cursor is the normal cursor
    void Update()
    {
        Vector2 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = cursorPos;

        if (Input.GetMouseButtonDown(0))
        {
            //rend.sprite = pointerCursor;
            Instantiate(clickEffect, transform.position, Quaternion.identity);
        } else if (Input.GetMouseButtonUp(0))
        {
            //rend.sprite = normalCursor;
        }

        if (timeBetweenSpawn <= 0)
        {
            Instantiate(trailEffect, cursorPos, Quaternion.identity);
            timeBetweenSpawn = 0.1f;
        } else
        {
            timeBetweenSpawn -= Time.deltaTime;
        }
    }
}
