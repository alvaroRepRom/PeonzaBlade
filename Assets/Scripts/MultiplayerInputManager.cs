using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerInputManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    private void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
       // playerInputManager.onPlayerJoined += PlayerInputManager_onPlayerJoined;
    }

    private void ChangePlayerPrefab()
    {
        //playerInputManager.playerPrefab = 
    }
}
