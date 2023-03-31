using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        MultiplayerInputManager.Instance.OnAllPlayersReady += Hide;
    }

    private void Hide() => gameObject.SetActive( false );
}
