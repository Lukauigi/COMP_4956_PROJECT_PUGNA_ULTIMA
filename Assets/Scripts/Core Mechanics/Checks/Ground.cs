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
    // Other scene objects to reference
    private GameplayAudioManager _audioManager;

    // If the player is on ground; touching a platform collider
    private bool onGround;

    // platform property
    private float friction;


    /// <summary>
    /// Start is called after Awake, and before Update.
    /// Generally used to reference other scene objects, after they have all been initialized.
    /// </summary>
    private void Start()
    {
        // cache other scene objects
        if (!_audioManager) _audioManager = GameObject.Find("SceneAudioManager").GetComponent<GameplayAudioManager>();
    }

    /// <summary>
    /// Triggers when player makes contact with the ground collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);
        
        // play landing sound
        if (Object.HasInputAuthority && onGround) _audioManager.RPC_PlayUniversalCharacterSFXAudio(PlayerActions.JumpLand.ToString());
    }

    /// <summary>
    /// Triggers when player stays on the ground collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        EvaluateCollision(collision);
        RetrieveFriction(collision);

    }

    /// <summary>
    /// Triggers when player leaves the ground collider
    /// </summary>
    /// <param name="collision"></param>
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

    /// <summary>
    /// Determines the ground collider's friction value.
    /// </summary>
    /// <param name="collision"></param>
    private void RetrieveFriction(Collision2D collision)
    {
        PhysicsMaterial2D material = collision.rigidbody.sharedMaterial;

        friction = 0;

        if (material != null)
        {
            friction = material.friction;
        }
    }

    /// <summary>
    /// Returns a bool for checking if the player is on the ground.
    /// </summary>
    /// <returns></returns>
    public bool GetOnGround()
    {
        return onGround;
    }

    /// <summary>
    /// Returns the friction value from the ground collider.
    /// </summary>
    /// <returns></returns>
    public float GetFriction()
    {
        return friction;
    }
}
