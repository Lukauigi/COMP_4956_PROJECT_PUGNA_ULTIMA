using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour //change to NetworkBehaviour
{

    public Collider2D[] attackHitboxes;

    //nov 7
    private GameObject attackArea = default;
    private bool attacking = false;
    private float timeToAttack = 0.25f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        attackArea = transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("G")) { iAttack(); }//If user presses G an attack is launched
                                                    //LaunchAttack(attackHitboxes[0]);

        if (attacking)
        {
            timer += Time.deltaTime;

            if (timer >= timeToAttack)
            {
                timer = 0;
                attacking = false;
                attackArea.SetActive(attacking);
            }
        }
    }

    private void LaunchAttack(Collider2D col)
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(col.bounds.center, col.bounds.extents, col.transform.rotation.x, LayerMask.GetMask("Hitbox"));
        foreach (Collider2D c in cols)
        {
            //float damage = 1;
            //Debug.Log("Hit Registered"); //Test if attack is going through

            if (c.transform.parent.parent == transform) // Check if attack hitbox is colliding with the player that used the attack
                continue;                               // If so do not register a hit and continue foreach loop

            Debug.Log(c.name); //If this shows the attack is hitting another player


        }

    }

    //nov 7
    private void iAttack()
    {
        attacking = true;
        attackArea.SetActive(attacking);
    }

    /*    public override void FixedUpdateNetwork()
        {

        }*/

}
