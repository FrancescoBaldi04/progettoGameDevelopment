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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
 void Start()
    {
        parassita = GetComponent<Parassita>();
    }

    void Update()
    { 
        movement = InputManager.movement;
        rb.linearVelocity = moveSpeed * movement;
        
        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        if (movement != Vector2.zero) {
            animator.SetFloat(lastHorizontal, movement.x);
            animator.SetFloat(lastVertical, movement.y);
        }
        
    if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            parassita.EsplosioneZipBomb();
        }
    }

}
