using System.Collections.Generic;
using UnityEngine;

public class SpawnBladeManager : MonoBehaviour
{
    public static SpawnBladeManager Instance;


    [SerializeField] private CharacterListSO characterListSO;

    private Dictionary<int, PlayerSelectionArgs> playerSelectionDict = new Dictionary<int, PlayerSelectionArgs>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MultiplayerInputManager.Instance.OnAllPlayersReady += Multi_OnAllPlayersReady;
    }

    private void Multi_OnAllPlayersReady()
    {
        for (int i = 0; i < playerSelectionDict.Count; i++)
        {
            int characterSelectedIndex = playerSelectionDict[i].characterSelectedIndex;

            GameObject blade = Instantiate( characterListSO.list[characterSelectedIndex].characterPrefab );

            GameInputs gameInputs = playerSelectionDict[i].gameInputs;
            gameInputs.SwitchUItoGameInputs();

            blade.GetComponent<BladeController>().SetGameInputs( gameInputs );
        }
    }

    public void SetPlayerCharacter( int playerIndex , PlayerSelectionArgs playerSelection)
    {
        if ( !playerSelectionDict.ContainsKey( playerIndex ) )
            playerSelectionDict.Add( playerIndex, playerSelection );
    }
    public void CancelPlayerCharacter( int playerIndex )
    {
        if ( playerSelectionDict.ContainsKey( playerIndex ) )
            playerSelectionDict.Remove( playerIndex );
    }
}

public class PlayerSelectionArgs
{
    public PlayerSelectionArgs( int characterSelectedIndex, GameInputs gameInputs )
    {
        this.characterSelectedIndex = characterSelectedIndex;
        this.gameInputs = gameInputs;
    }

    public int characterSelectedIndex;
    public GameInputs gameInputs;
}