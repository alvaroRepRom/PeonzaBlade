using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerJoinedPrefabUI : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private TextMeshProUGUI playerIndexText;
    [SerializeField] private Button characterSelectionButton;

    [Header("Characters SO")]
    [SerializeField] private CharacterStatsSO[] charactersStatsSO;

    GameInputs gameInputs;
    private int playerIndex;
    private int characterSelectIndex = 0;
    private GameInputs.NavDirection navDirection;


    private void Update()
    {
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

            if ( characterSelectIndex >= charactersStatsSO.Length )
                characterSelectIndex = 0;
            else
            if ( characterSelectIndex < 0 )
                characterSelectIndex = charactersStatsSO.Length - 1;

            SetPlayerImage();
            navDirection = GameInputs.NavDirection.NONE;
        }
    }

    public void SetPlayerImage()
    {
        playerImage.sprite = charactersStatsSO[characterSelectIndex].characterImage;
    }

    public void SetPlayerInput( GameInputs playerGameInputs , int playerIndex )
    {
        gameInputs = playerGameInputs;
        playerGameInputs.OnNavigationPerformed += GameInputs_OnNavigationPerformed;
        this.playerIndex = playerIndex + 1;
        playerIndexText.text = "Player " + this.playerIndex;
        gameObject.name = "Player " + this.playerIndex;
    }

    private void GameInputs_OnNavigationPerformed( GameInputs.NavDirection dir )
    {
        navDirection = dir;
    }
}
