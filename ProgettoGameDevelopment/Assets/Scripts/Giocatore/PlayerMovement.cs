using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb; 
    private Animator animator;
    private Vector2 movement;
    private const string horizontal = "Horizontal"; // nomi parametri float che ho usato nell'animator
    private const string vertical = "Vertical"; 
    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";
   private Parassita parassita;
   private PlayerJump playerJump;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
 void Start()
    {
        parassita = GetComponent<Parassita>();
        playerJump = GetComponent<PlayerJump>();
    }

    void Update()
    {
        if (playerJump != null && playerJump.isInAir) {
            return;
        }
        
        movement = InputManager.movement;
        
        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        if (movement != Vector2.zero) {
            animator.SetFloat(lastHorizontal, movement.x);
            animator.SetFloat(lastVertical, movement.y);
        }

        if (playerJump != null && playerJump.isCharging){
            rb.linearVelocity = Vector2.zero;  // se sta caricando il salto blocco il movimento 
        }else{
            rb.linearVelocity = moveSpeed * movement;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame) {
            parassita.EsplosioneZipBomb();
        }
    }

}
