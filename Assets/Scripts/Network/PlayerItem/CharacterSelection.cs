using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Author: Roswell Doria
/// Date: 2022-12-03
/// 
/// This class is responsible for setting the selected characters.
///
/// </summary>
public class CharacterSelection : NetworkBehaviour
{

    public NetworkObject[] Characters;
    public NetworkObject CharacterRef;
    public int selectedCharacter = 0;

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responsible for selecting the next character.
    ///
    /// </summary>
    public void NextCharacter()
    {
        selectedCharacter = (selectedCharacter + 1) % Characters.Length;
        CharacterRef = Characters[selectedCharacter];
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responsible for selecting the previous character.
    ///
    /// </summary>
    public void PreviousCharacter()
    {
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += Characters.Length;
        }
        CharacterRef = Characters[selectedCharacter];
    }

    /// <summary>
    /// Author: Roswell Doria
    /// Date: 2022-12-03
    /// 
    /// This function is responsible for setting selected charcter into PlayerPrefs
    /// when they are ready to play the game.
    ///
    /// </summary>
    public void ReadyGame()
    {
        PlayerPrefs.SetInt("selected character", selectedCharacter);
        SceneManager.LoadScene("Networked Fight");
    }

}
