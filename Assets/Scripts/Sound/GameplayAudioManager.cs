using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public enum PlayerActions
{
    Attack,
    ReceiveDamage,
    Move,
    Death,
    Jump,
    JumpLand,
    Dodge
}

public class GameplayAudioManager : NetworkBehaviour
{
    public static GameplayAudioManager Instance = null;
    protected GameManager _gameManager;

    [SerializeField] private AudioClip[] attackSoundPool;
    [SerializeField] private AudioClip[] receiveDamageSoundPool;
    [SerializeField] private AudioClip[] moveSoundPool;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip jumpLandSound;
    [SerializeField] private AudioClip dodgeSound;
    private Dictionary<string, AudioClip[]> hostPlayerAudio;
    private Dictionary<string, AudioClip[]> clientPlayerAudio;
    private Dictionary<string, AudioClip> universalPlayerAudio;
    [SerializeField] public AudioSource sfxAudioSource;
    [SerializeField] public AudioSource musicAudioSource;

    private void Awake()
    {
        Instance = this;
        hostPlayerAudio = new Dictionary<string, AudioClip[]> {
            { PlayerActions.Attack.ToString(), attackSoundPool },
            { PlayerActions.ReceiveDamage.ToString(), receiveDamageSoundPool },
            { PlayerActions.Move.ToString(), moveSoundPool }
        };
        clientPlayerAudio = new Dictionary<string, AudioClip[]> {
            { PlayerActions.Attack.ToString(), attackSoundPool },
            { PlayerActions.ReceiveDamage.ToString(), receiveDamageSoundPool },
            { PlayerActions.Move.ToString(), moveSoundPool }
        };
        universalPlayerAudio = new Dictionary<string, AudioClip>
        {
            { PlayerActions.Death.ToString(), deathSound },
            { PlayerActions.Dodge.ToString(), dodgeSound },
            { PlayerActions.Jump.ToString(), jumpSound },
            { PlayerActions.JumpLand.ToString(), jumpLandSound }
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        CacheOtherObjects();
    }

    // Helper method to initialize OTHER game objects and their components
    private void CacheOtherObjects()
    {
        if (!_gameManager) _gameManager = GameManager.Manager;
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlaySpecificCharatcerSFXAudio(int playerRefId, string playerAction, bool isLoopingAudio)
    {
        sfxAudioSource.loop = isLoopingAudio;
        print("Audio Call " + playerAction);
        sfxAudioSource.PlayOneShot(hostPlayerAudio[playerAction][0]);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlayUniversalCharatcerSFXAudio(string playerAction, bool isLoopingAudio)
    {
        sfxAudioSource.loop = isLoopingAudio;
        print("Audio Call " + playerAction);
        sfxAudioSource.PlayOneShot(universalPlayerAudio[playerAction]);
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_StopAudio()
    {
        sfxAudioSource.Stop();
    }

    [Rpc(sources: RpcSources.All, targets: RpcTargets.All)]
    public void RPC_PlayMenuSFXAudio()
    {

    }

}
