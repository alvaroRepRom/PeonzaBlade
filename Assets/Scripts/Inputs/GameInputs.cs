using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs : MonoBehaviour
{
    public event Action OnAttackPerformed;
    public event Action OnDefensePerformed;
    public event Action OnPausePerformed;
    public event Action OnJumpPerformed;

    public event Action<NavDirection> OnNavigationPerformed;
    public enum NavDirection { NONE, UP, RIGHT, DOWN, LEFT }
    public event Action OnSubmitPerformed;
    public event Action OnCancelPerformed;


    private PlayerInput playerInput;
    private InputAction moveAction;

    [SerializeField] private int playerIndex = -1;

    public int PlayerIndex => playerIndex;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerIndex = playerInput.playerIndex;

        // Game
        moveAction = playerInput.actions.FindAction( "Move" );
        playerInput.actions.FindAction( "Attack" ).performed += Attack_performed;
        playerInput.actions.FindAction( "Defense" ).performed += Defense_performed;
        playerInput.actions.FindAction( "Pause" ).performed += Pause_performed;
        playerInput.actions.FindAction( "Jump" ).performed += Jump_performed;

        // UI
        playerInput.actions.FindAction( "Navigation" ).performed += Navigation_performed;
        playerInput.actions.FindAction( "Submit" ).performed += Submit_performed;
        playerInput.actions.FindAction( "Cancel" ).performed += CancelUI_performed;
        playerInput.SwitchCurrentActionMap( "UI" );
    }

    public void SwitchUItoGameInputs()
    {
        //playerInput.currentActionMap.Disable();
        playerInput.SwitchCurrentActionMap( "Blade" );
        //playerInput.currentActionMap.Enable();
        // Game
        //moveAction = playerInput.actions.FindAction( "Move" );
        //playerInput.actions.FindAction( "Attack" ).performed += Attack_performed;
        //playerInput.actions.FindAction( "Defense" ).performed += Defense_performed;
        //playerInput.actions.FindAction( "Pause" ).performed += Pause_performed;
        //playerInput.actions.FindAction( "Jump" ).performed += Jump_performed;
    }

    private void Navigation_performed( InputAction.CallbackContext ctx )
    {
        Vector2 navigationInput = ctx.ReadValue<Vector2>().normalized;
        if ( navigationInput.x > 0.5f ) // Right
            OnNavigationPerformed?.Invoke( NavDirection.RIGHT );
        else
        if ( navigationInput.x < -0.5f ) // Left
            OnNavigationPerformed?.Invoke( NavDirection.LEFT );
        else
        if ( navigationInput.y > 0.5f ) // Up
            OnNavigationPerformed?.Invoke( NavDirection.UP );
        else
        if ( navigationInput.y < -0.5f ) // Down
            OnNavigationPerformed?.Invoke( NavDirection.UP );
    }

    private void Submit_performed( InputAction.CallbackContext obj )
    {
        OnSubmitPerformed?.Invoke();
    }

    private void CancelUI_performed( InputAction.CallbackContext obj )
    {
        OnCancelPerformed?.Invoke();
    }

    private void Jump_performed( InputAction.CallbackContext ctx )
    {
        OnJumpPerformed?.Invoke();
    }

    private void Pause_performed( InputAction.CallbackContext ctx )
    {
        OnPausePerformed?.Invoke();
    }

    private void Defense_performed( InputAction.CallbackContext ctx )
    {
        OnDefensePerformed?.Invoke();
    }

    private void Attack_performed( InputAction.CallbackContext ctx )
    {
        OnAttackPerformed?.Invoke();
    }

    public Vector2 MovementNormalized()
    {
        return moveAction.ReadValue<Vector2>().normalized;
    }


    private void OnDestroy()
    {
        playerInput.actions.FindAction( "Attack" ).performed -= Attack_performed;
        playerInput.actions.FindAction( "Defense" ).performed -= Defense_performed;
        playerInput.actions.FindAction( "Pause" ).performed -= Pause_performed;
        playerInput.actions.FindAction( "Jump" ).performed -= Jump_performed;
    }
}
