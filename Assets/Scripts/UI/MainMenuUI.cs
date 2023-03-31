using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        MultiplayerInputManager.Instance.OnAllPlayersReady += Hide;
    }

    private void Update()
    {
        if ( Keyboard.current.escapeKey.isPressed )
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

    }

    private void Hide() => gameObject.SetActive( false );
}
