using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Diagnostics;

/// <summary>
/// Class that determines if the player is on a light platform.
/// Author(s): Matthew Kan, Jason Cheung
/// Date: Nov 10 2022
/// </summary>
public class LightPlatform : NetworkBehaviour
{
    // Light platform that the player is currently standing on
    private GameObject _currentLightPlatform;
    public GameObject CurrentLightPlatform => _currentLightPlatform; // getter

    /// <summary>
    /// Triggers when player makes contact with the light platform
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TwoWayPlatform") &&
            _currentLightPlatform != collision.gameObject)
        {
            _currentLightPlatform = collision.gameObject;
        }
    }

    /// <summary>
    /// Triggers when player stays on the light platform
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TwoWayPlatform") &&
            _currentLightPlatform != collision.gameObject)
        {
            _currentLightPlatform = collision.gameObject;
        }
    }

    /// <summary>
    /// Triggers when player leave the light platform
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("TwoWayPlatform") &&
            _currentLightPlatform == collision.gameObject)
        {
            _currentLightPlatform = null;
        }
    }
}
