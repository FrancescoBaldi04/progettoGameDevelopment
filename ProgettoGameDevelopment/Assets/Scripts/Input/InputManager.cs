using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement {get; private set;} 
    public static bool chargeStarted {get; private set;}    
    public static bool chargeHeld {get; private set;}
    public static bool chargeReleased{get; private set;}

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction chargeAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        chargeAction = playerInput.actions["ChargeAndJump"];
    }

    void Update()
    {
        if (Time.timeScale == 0f) // Ignore inputs when the game is paused
        {
            movement = Vector2.zero;
            chargeStarted = false;
            chargeHeld = false;
            chargeReleased = false;
            return;
        }
        
        movement = moveAction.ReadValue<Vector2>(); // Poll current input values

        chargeStarted = chargeAction.WasPressedThisFrame();
        chargeHeld = chargeAction.IsPressed();
        chargeReleased = chargeAction.WasReleasedThisFrame();
    }
}
