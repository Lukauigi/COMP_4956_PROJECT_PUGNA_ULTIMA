using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

/// <summary>
/// Class that determines if we are on the ground or in the air
/// /// Author(s): Jun Earl Solomon
/// Date: Oct 29 2022
/// Source(s):
///     The ULTIMATE 2D Character CONTROLLER in UNITY (2021): https://youtu.be/lcw6nuc2uaU
/// Changes: November 22, 2022
/// - Add logic for RPC call for sound effect method.
/// </summary>
public class Ground : NetworkBehaviour
{
    private GameObject audioManager;
    private bool onGround;
    private float friction;

    // reference the animator controller for player
    //public Animator animator;

    private void Start()
    {
        this.audioManager = GameObject.Find("SceneAudioManager");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
        if (Object.HasInputAuthority && onGround) audioManager.GetComponent<GameplayAudioManager>().RPC_PlayUniversalCharatcerSFXAudio(PlayerActions.JumpLand.ToString());
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        onGround = false;
        friction = 0;
    }

    /// <summary>
    /// Determines whether the point that the game object is colliding with is the ground
    /// </summary>
    /// <param name="collision">A collision</param>
    private void EvaluateCollision(Collision2D collision)
    {
        for (int i=0; i < collision.contactCount; i++)
        {
            Vector2 normal = collision.GetContact(i).normal;
            onGround |= normal.y >= 0.9f;
        }
        
    }


    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;

        friction = 0;

        if (material != null)
        {
            friction = material.friction;
        }
    }

    public bool GetOnGround()
    {
        return onGround;
    }

    public float GetFriction()
    {
        return friction;
    }
}
