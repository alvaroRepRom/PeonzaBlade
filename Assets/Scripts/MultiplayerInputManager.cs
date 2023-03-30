using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerInputManager : MonoBehaviour
{
    public event Action<PlayerInput> OnPlayerJoined;


    public static MultiplayerInputManager Instance { get; private set; }

    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        Instance = this;

        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += PlayerInputManager_onPlayerJoined;
    }

    private void PlayerInputManager_onPlayerJoined( PlayerInput playerInput )
    {
        OnPlayerJoined?.Invoke( playerInput );
    }
}
