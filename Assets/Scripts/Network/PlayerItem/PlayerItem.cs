using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerItem : NetworkBehaviour
{

    public Text playerName;
    public Image backgroundImage;
    public Color highlightColor;
    public GameObject previousBtn;
    public GameObject nextBtn;

    public Image Avatar;
    [SerializeField] private NetworkObject _myNetworkObject;

    //[SerializeField] private NetworkObject[] characters;
    [SerializeField] private Color[] characters;
    private int selectedCharacter = 0;


    // Start is called before the first frame update
    void Start()
    {

        Avatar.color = characters[selectedCharacter];
    }

    //public void setPlayerInfo(Player _player)
    //{
    //    playerName.text = _player.name;
    //}

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_NextButtonFunction()
    {
        if(Object.HasInputAuthority)
        {

            selectedCharacter = (selectedCharacter + 1) % characters.Length;
            Avatar.color = characters[selectedCharacter];

        }
    }

    [Rpc(sources: RpcSources.InputAuthority, targets: RpcTargets.All)]
    public void RPC_PreviousButtonFunction()
    {
        if (Object.HasInputAuthority)
        {
            selectedCharacter--;
            if (selectedCharacter < 0)
            {
                selectedCharacter = characters.Length - 1;
            }
            Avatar.color = characters[selectedCharacter];
        }
    }

    public override void Spawned()
    {
        Debug.Log("YOU SET ACTIVE");
        //backgroundImage.color = highlightColor;
        previousBtn.SetActive(true);
        nextBtn.SetActive(true);
    }

}
