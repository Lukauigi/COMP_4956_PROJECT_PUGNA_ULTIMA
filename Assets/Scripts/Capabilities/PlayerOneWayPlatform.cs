using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class PlayerOneWayPlatform : NetworkBehaviour
{

    /*private Rigidbody2D body;
    private Ground ground;
    private Vector2 velocity;*/

    // Game object for platform on screen
    private GameObject currentOneWayPlatform;

/*    [SerializeField] private InputController input = null;
*/
    // Player BoxCollider2D field
    [SerializeField] private BoxCollider2D playerCollider;


    void Awake()
    {
       /* body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Ground>();*/
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Calls this method every frame but is reflected on the network
    public override void FixedUpdateNetwork()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (currentOneWayPlatform != null)
            {
                StartCoroutine(DisableCollision());
            }
        }
    }

    // Triggers when player makes contact with the platform
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }

    // Triggers when player passes through the platform from above
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    // Method used to ignore the collision field of the platform 
    private IEnumerator DisableCollision()
    {
        print("disable collision method called");
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
        print("surface back on");


    }
}
