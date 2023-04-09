using UnityEngine;

public class JoinedPlayersUI : MonoBehaviour
{
    [SerializeField] private GameObject playerUIPrefab;
    [SerializeField] private GameObject pushToJoinObj;

    private int numOfPlayersConnected = 0;

    private void Start()
    {
        MultiplayerInputManager.Instance.OnPlayerJoined += MultiplayerInputManager_OnPlayerJoined;
        MultiplayerInputManager.Instance.OnPlayerLeft += MultiplayerInputManager_OnPlayerLeft;
    }

    private void MultiplayerInputManager_OnPlayerLeft( int playerIndex )
    {
        pushToJoinObj.SetActive( true );
        pushToJoinObj.transform.SetAsLastSibling();
    }

    private void MultiplayerInputManager_OnPlayerJoined( GameInputs playerGameInputs , int playerIndex )
    {
        GameObject playerUIObj = Instantiate( playerUIPrefab , transform );
        PlayerJoinedPrefabUI playerJoinedPrefabUI = playerUIObj.GetComponent<PlayerJoinedPrefabUI>();
        playerJoinedPrefabUI.SetPlayerInput( playerGameInputs , playerIndex );


        pushToJoinObj.transform.SetAsLastSibling();
        numOfPlayersConnected++;

        if ( numOfPlayersConnected >= 4 )
            pushToJoinObj.SetActive( false );
    }

    private void OnDestroy()
    {
        if ( MultiplayerInputManager.Instance )
        {
            MultiplayerInputManager.Instance.OnPlayerJoined -= MultiplayerInputManager_OnPlayerJoined;
            MultiplayerInputManager.Instance.OnPlayerLeft -= MultiplayerInputManager_OnPlayerLeft;
        }
    }
}
