using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerJoinedPrefabUI : MonoBehaviour
{
    [SerializeField] private RawImage playerImage;
    [SerializeField] private TextMeshProUGUI playerIndexText;

    private int playerIndex;

    public void SetPlayerImage( Texture characterTex )
    {
        playerImage.texture = characterTex;
    }

    public void SetPlayerIndex( int playerIndex )
    {
        this.playerIndex = playerIndex + 1;
        playerIndexText.text = "Player " + this.playerIndex;
        gameObject.name = "Player " + this.playerIndex;
    }
}
