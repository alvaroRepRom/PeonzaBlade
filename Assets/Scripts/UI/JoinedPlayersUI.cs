using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class JoinedPlayersUI : MonoBehaviour
{
    [SerializeField] private GameObject playerUIPrefab;

    private void Start()
    {
        MultiplayerInputManager.Instance.OnPlayerJoined += MultiplayerInputManager_OnPlayerJoined;
    }

    private void MultiplayerInputManager_OnPlayerJoined( PlayerInput playerInput )
    {
        GameObject playerUIObj = Instantiate( playerUIPrefab , transform );
        PlayerJoinedPrefabUI playerJoinedPrefabUI = playerUIObj.GetComponent<PlayerJoinedPrefabUI>();
        playerJoinedPrefabUI.SetPlayerIndex( playerInput.playerIndex );
    }
}
