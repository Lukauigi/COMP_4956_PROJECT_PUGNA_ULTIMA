using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : NetworkBehaviour
{

    public NetworkObject[] characters;
    public NetworkObject _characterRef;
    public int selectedCharacter = 0;

    public void NextCharacter()
    {
        selectedCharacter = (selectedCharacter + 1) % characters.Length;
        _characterRef = characters[selectedCharacter];
    }

    public void PreviousCharacter()
    {
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter += characters.Length;
        }
        _characterRef = characters[selectedCharacter];
    }

    public void ReadyGame()
    {
        PlayerPrefs.SetInt("selected character", selectedCharacter);
        SceneManager.LoadScene("Networked Fight");
    }

}
