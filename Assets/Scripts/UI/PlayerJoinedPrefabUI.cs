using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerJoinedPrefabUI : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerIndexText;
    [SerializeField] private Button characterSelectionButton;
    [SerializeField] private GameObject readyText;

    [Header("Characters SO")]
    [SerializeField] private CharacterListSO characterListSO;

    GameInputs gameInputs;
    private int playerIndex;
    private int characterSelectIndex = 0;
    private GameInputs.NavDirection navDirection;

    bool isReady = false;

    private void Awake()
    {
        MultiplayerInputManager.Instance.OnPlayerLeft += MultiplayerInputManager_OnPlayerLeft;
    }

    private void MultiplayerInputManager_OnPlayerLeft( int playerLeftIndex )
    {
        if ( playerIndex.Equals( playerLeftIndex ) )
        {
            SpawnBladeManager.Instance.CancelPlayerCharacter( playerIndex );
            Destroy( gameObject );
        }
    }

    private void Update()
    {
        if ( isReady ) return;

        int addIndex = 0;
        switch ( navDirection )
        {
            case GameInputs.NavDirection.UP:
                break;
            case GameInputs.NavDirection.RIGHT:
                addIndex++;
                break;
            case GameInputs.NavDirection.DOWN:
                break;
            case GameInputs.NavDirection.LEFT:
                addIndex--;
                break;
        }

        if ( addIndex != 0 )
        {
            characterSelectIndex += addIndex;

            if ( characterSelectIndex >= characterListSO.list.Length )
                characterSelectIndex = 0;
            else
            if ( characterSelectIndex < 0 )
                characterSelectIndex = characterListSO.list.Length - 1;

            SetPlayerImage();
            navDirection = GameInputs.NavDirection.NONE;
        }
    }

    public void SetPlayerImage()
    {
        playerImage.sprite = characterListSO.list[characterSelectIndex].characterImage;
    }

    public void SetPlayerInput( GameInputs playerGameInputs , int playerIndex )
    {
        gameInputs = playerGameInputs;
        playerGameInputs.OnNavigationPerformed += GameInputs_OnNavigationPerformed;
        playerGameInputs.OnSubmitPerformed += GameInputs_OnSubmitPerformed;
        playerGameInputs.OnCancelPerformed += GameInputs_OnCancelPerformed;

        this.playerIndex = playerIndex;
        playerIndexText.text = "Player " + ( playerIndex + 1 );
        gameObject.name = "Player " + ( playerIndex + 1 );
    }

    
    private void GameInputs_OnSubmitPerformed()
    {
        isReady = true;
        characterSelectionButton.interactable = false;
        readyText.SetActive( true );

        SpawnBladeManager.Instance.SetPlayerCharacter( playerIndex , new PlayerSelectionArgs( 
                                                                    characterSelectIndex , gameInputs ) );

        MultiplayerInputManager.Instance.SetPlayerReady( playerIndex );
    }

    private void GameInputs_OnCancelPerformed()
    {
        isReady = false;
        characterSelectionButton.interactable = true;
        readyText.SetActive( true );
        MultiplayerInputManager.Instance.PlayerNotReady( playerIndex );
        SpawnBladeManager.Instance.CancelPlayerCharacter( playerIndex );
    }

    private void GameInputs_OnNavigationPerformed( GameInputs.NavDirection dir )
    {
        navDirection = dir;
    }

    private void OnDestroy()
    {
        if ( MultiplayerInputManager.Instance != null )
            MultiplayerInputManager.Instance.OnPlayerLeft -= MultiplayerInputManager_OnPlayerLeft;

        if ( gameInputs != null )
        {
            gameInputs.OnNavigationPerformed -= GameInputs_OnNavigationPerformed;
            gameInputs.OnSubmitPerformed -= GameInputs_OnSubmitPerformed;
            gameInputs.OnCancelPerformed -= GameInputs_OnCancelPerformed;
        }
    }
}
