using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerInputManager : MonoBehaviour
{
    public event Action OnAllPlayersReady;
    public event Action<GameInputs, int> OnPlayerJoined;
    public Action<int> OnPlayerLeft;

    public static MultiplayerInputManager Instance { get; private set; }


    private Dictionary<int, bool> playersReadyDict = new Dictionary<int, bool>();
    private PlayerInputManager playerInputManager;


    private void Awake()
    {
        Instance = this;

        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += PlayerInputManager_onPlayerJoined;
        playerInputManager.onPlayerLeft += PlayerInputManager_onPlayerLeft;
    }

    private void PlayerInputManager_onPlayerLeft( PlayerInput playerInput )
    {
        playersReadyDict.Remove( playerInput.playerIndex );
        OnPlayerLeft?.Invoke( playerInput.playerIndex );
    }

    private void PlayerInputManager_onPlayerJoined( PlayerInput playerInput )
    {
        playersReadyDict.Add( playerInput.playerIndex , false );

        GameInputs playerGameInputs = playerInput.gameObject.GetComponent<GameInputs>();
        OnPlayerJoined?.Invoke( playerGameInputs , playerInput.playerIndex );
    }

    public void SetPlayerReady( int playerIndex )
    {
        playersReadyDict[playerIndex] = true;

        int playersJoined = 0;
        foreach ( bool playerReady in playersReadyDict.Values )
            if ( playerReady )
                playersJoined++;

        if ( playersReadyDict.Count == playersJoined )
            OnAllPlayersReady?.Invoke();
    }

    public void PlayerNotReady( int playerIndex ) => playersReadyDict[playerIndex] = false;

    public int GetMaxNumOfPlayers() => playerInputManager.maxPlayerCount;
}
