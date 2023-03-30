using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputs : MonoBehaviour
{
    public event Action OnAttackPerformed;
    public event Action OnDefensePerformed;
    public event Action OnPausePerformed;
    public event Action OnJumpPerformed;


    private PlayerInput playerInput;
    private InputAction moveAction;

    [SerializeField] private int playerIndex = -1;


    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerIndex = playerInput.playerIndex;

        moveAction = playerInput.actions.FindAction( "Move" );
        playerInput.actions.FindAction( "Attack" ).performed += Attack_performed;
        playerInput.actions.FindAction( "Defense" ).performed += Defense_performed;
        playerInput.actions.FindAction( "Pause" ).performed += Pause_performed;
        playerInput.actions.FindAction( "Jump" ).performed += Jump_performed;
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
