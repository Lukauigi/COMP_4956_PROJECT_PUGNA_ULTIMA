using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour //change to NetworkBehaviour
{

    public Collider[] attackHitboxes;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G)) //If user presses G an attack is launched
            LaunchAttack(attackHitboxes[0]);
    }

    private void LaunchAttack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
            Debug.Log(c.name);
          /*  if (c.transform.parent.parent == transform) // Check if attack hitbox is colliding with the player that used the attack
                continue;   */                            // If so do not register a hit and continue foreach loop
        }
       
    }


    /*    public override void FixedUpdateNetwork()
        {

        }*/

}
