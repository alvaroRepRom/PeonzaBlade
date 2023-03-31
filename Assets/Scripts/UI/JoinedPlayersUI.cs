using UnityEngine;

public class JoinedPlayersUI : MonoBehaviour
{
    [SerializeField] private GameObject playerUIPrefab;

    private void Start()
    {
        MultiplayerInputManager.Instance.OnPlayerJoined += MultiplayerInputManager_OnPlayerJoined;
    }

    private void MultiplayerInputManager_OnPlayerJoined( GameInputs playerGameInputs , int playerIndex )
    {
        GameObject playerUIObj = Instantiate( playerUIPrefab , transform );
        PlayerJoinedPrefabUI playerJoinedPrefabUI = playerUIObj.GetComponent<PlayerJoinedPrefabUI>();
        playerJoinedPrefabUI.SetPlayerInput( playerGameInputs , playerIndex );
    }
}
