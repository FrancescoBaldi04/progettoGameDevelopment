using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
   public static Vector2 movement; // static in modo da accedere ai valori x e y relativi al movimento anche dagli altri script

   private PlayerInput playerInput;
   private InputAction moveAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];  // nome azione che ho creato su unity
    }

    void Update()
    {
        movement = moveAction.ReadValue<Vector2>();
    }
}
