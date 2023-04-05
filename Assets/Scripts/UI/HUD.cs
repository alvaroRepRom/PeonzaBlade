using UnityEngine;

public class HUD : MonoBehaviour
{
    private void Start()
    {
        MultiplayerInputManager.Instance.OnAllPlayersReady += Show;
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive( true );
    }

    private void Hide()
    {
        gameObject.SetActive( false );
    }
}
