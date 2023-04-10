using System;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBladeManager : MonoBehaviour
{
    public event Action OnBladesLaunched;

    public static SpawnBladeManager Instance;


    [SerializeField] private CharacterListSO characterListSO;
    [SerializeField] private SingleHUD[] playerSingleHUDs;
    [SerializeField] private ColorListSO colorListSO;


    private Dictionary<int, PlayerSelectionArgs> playerSelectionDict = 
        new Dictionary<int, PlayerSelectionArgs>();
    private Vector3[] spawnPoints = new Vector3[] {
        new Vector3(-10,0, 10), new Vector3(10,0, 10),
        new Vector3(-10,0,-10), new Vector3(10,0,-10),
    };

    private void Awake() => Instance = this;
    private void Start() => MultiplayerInputManager.Instance.OnAllPlayersReady += Multi_OnAllPlayersReady;

    private void Multi_OnAllPlayersReady()
    {
        SetPlayerBlades();
        SetCPUBlades();
        OnBladesLaunched?.Invoke();
    }

    private void SetPlayerBlades()
    {
        for ( int i = 0; i < playerSelectionDict.Count; i++ )
        {
            int characterSelectedIndex = playerSelectionDict[i].characterSelectedIndex;

            GameObject bladeObj = Instantiate( characterListSO.list[characterSelectedIndex].characterPrefab ,
                                            spawnPoints[i] , Quaternion.identity );

            GameInputs gameInputs = playerSelectionDict[i].gameInputs;
            gameInputs.SwitchUItoGameInputs();


            BladeController bladeController = bladeObj.GetComponent<BladeController>();
            bladeController.SetGameInputs( gameInputs );
            bladeController.SetSingleHUD( playerSingleHUDs[i] );

            bladeObj.GetComponent<PlayerColorUI>().SetPlayerColor( colorListSO.colorList[i] );

            playerSingleHUDs[i].SetPlayerHUD( 
                characterListSO.list[characterSelectedIndex].characterImage,
                colorListSO.colorList[i],
                50,
                (int)characterListSO.list[characterSelectedIndex].maxRotationSpeed );
        }
    }

    private void SetCPUBlades()
    {
        int maxNumPlayer = 4;
        int cpuBlades = maxNumPlayer - playerSelectionDict.Count;
        for ( int i = 0; i < cpuBlades; i++ )
        {
            int randomCharacterIndex = UnityEngine.Random.Range( 0 , characterListSO.list.Length );
            GameObject blade = Instantiate( characterListSO.list[randomCharacterIndex].characterPrefab ,
                                            spawnPoints[playerSelectionDict.Count + i] , Quaternion.identity );

            CPUController cpuController = blade.GetComponent<CPUController>();
            cpuController.SetCPUBlade( i + playerSelectionDict.Count );
            cpuController.SetSingleHUD( playerSingleHUDs[i + playerSelectionDict.Count] );

            blade.GetComponent<PlayerColorUI>().SetPlayerColor( colorListSO.colorList[i + playerSelectionDict.Count] );

            playerSingleHUDs[i + playerSelectionDict.Count].SetPlayerHUD(
                characterListSO.list[randomCharacterIndex].characterImage ,
                colorListSO.colorList[i + playerSelectionDict.Count] ,
                100 ,
                ( int )characterListSO.list[randomCharacterIndex].maxRotationSpeed );
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