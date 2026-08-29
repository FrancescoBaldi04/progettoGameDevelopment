using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static Vector2 movement {get; private set;} // serve per fare in modo che gli altri script possono leggere la variabile ma non possano modificarla
    public static bool chargeStarted {get; private set;}    
    public static bool chargeHeld {get; private set;}
    public static bool chargeReleased{get; private set;}

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction chargeAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];  // nome azione che ho creato su unity
        chargeAction = playerInput.actions["ChargeAndJump"];
    }

    void Update()
    {
        if (Time.timeScale == 0f) // se siamo nel menu di pausa non legge gli input
        {
            movement = Vector2.zero;
            chargeStarted = false;
            chargeHeld = false;
            chargeReleased = false;
            return;
        }
        
        movement = moveAction.ReadValue<Vector2>();

        chargeStarted = chargeAction.WasPressedThisFrame();
        chargeHeld = chargeAction.IsPressed();
        chargeReleased = chargeAction.WasReleasedThisFrame();
    }
}
