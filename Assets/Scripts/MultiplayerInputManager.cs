using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerInputManager : MonoBehaviour
{
    public event Action<GameInputs, int> OnPlayerJoined;


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
        GameInputs playerGameInputs = playerInput.gameObject.GetComponent<GameInputs>();
        OnPlayerJoined?.Invoke( playerGameInputs , playerInput.playerIndex );
    }
}
