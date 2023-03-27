using System;
using UnityEngine;

public class GameInputs : MonoBehaviour
{
    public static GameInputs Instance {  get; private set; }


    public event Action OnAttackPerformed;
    public event Action OnDefensePerformed;
    public event Action OnPausePerformed;
    public event Action OnJumpPerformed;


    private PlayerControls playerControls;


    private void Awake()
    {
        Instance = this;
        playerControls = new PlayerControls();
        EnablePlayer();
    }


    private void EnablePlayer()
    {
        playerControls.Blade.Enable();
        playerControls.Blade.Attack.performed += Attack_performed;
        playerControls.Blade.Defense.performed += Defense_performed;
        playerControls.Blade.Pause.performed += Pause_performed;
        playerControls.Blade.Jump.performed += Jump_performed;
    }

    private void Jump_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj )
    {
        OnJumpPerformed?.Invoke();
    }

    private void Pause_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj )
    {
        OnPausePerformed?.Invoke();
    }

    private void Defense_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj )
    {
        OnDefensePerformed?.Invoke();
    }

    private void Attack_performed( UnityEngine.InputSystem.InputAction.CallbackContext obj )
    {
        OnAttackPerformed?.Invoke();
    }

    public Vector2 MovementNormalized()
    {
        return playerControls.Blade.Move.ReadValue<Vector2>().normalized;
    }


    private void OnDestroy()
    {
        playerControls.Blade.Attack.performed -= Attack_performed;
        playerControls.Blade.Defense.performed -= Defense_performed;
        playerControls.Blade.Pause.performed -= Pause_performed;
        playerControls.Blade.Jump.performed -= Jump_performed;
        playerControls.Blade.Disable();
    }
}
