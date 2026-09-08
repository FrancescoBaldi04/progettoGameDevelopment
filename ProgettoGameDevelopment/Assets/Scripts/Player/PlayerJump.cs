using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float maxChargeTime = 1.5f;
    [SerializeField] private float baseJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 15f;

    public bool isCharging {get; private set;} = false; 
    public bool isInAir {get; private set;} = false;
    public bool isDead {get; private set;} = false;

    private Rigidbody2D rb;
    private Animator animator;
    private float chargeTimer = 0f;
    private int currentChargeState = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        // Prevent charging input when airborne or dead
        if (isInAir || isDead) return;

        chargeInput();
    }

    private void chargeInput()
    {
        // Start charging input
        if (InputManager.chargeStarted){ 
            isCharging = true;
            chargeTimer = 0f;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isCharging", true);
        }

        if (InputManager.chargeHeld && isCharging){ // Holding input
            chargeTimer += Time.deltaTime;
            float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime);

            if (chargePercent < 0.33f) { // 3 distinct charge tiers
                currentChargeState = 1;
            }else if (chargePercent < 0.66f) {
                currentChargeState = 2;
            }else{
                currentChargeState = 3;   
            }

            animator.SetInteger("chargeState", currentChargeState);
        }

        // Release to jump
        if (InputManager.chargeReleased && isCharging){
            Jump();
        }
    }

    private void Jump(){
        isCharging = false;
        isInAir = true;

        // Calculate trajectory based on last direction input 
        float lastX = animator.GetFloat("LastHorizontal");
        float lastY = animator.GetFloat("LastVertical");
        Vector2 jumpDirection = new Vector2(lastX, lastY).normalized; 

        if (jumpDirection == Vector2.zero) jumpDirection = Vector2.down; 

        // Scale jump force proportionally to charge state
        float finalForce = Mathf.Lerp(baseJumpForce, maxJumpForce, (float)currentChargeState / 3f);
        rb.AddForce(jumpDirection * finalForce, ForceMode2D.Impulse); 

        animator.SetBool("isCharging", false);
        animator.SetTrigger("Jump");
        currentChargeState = 0;
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (isInAir){
            if (collision.gameObject.CompareTag("Npc")){ 
                PossessNpc(collision.gameObject);
            }else{
                Die();   
            }
        }
    }

    private void PossessNpc(GameObject Npc){
        isInAir = false;
        
        Parasite parasite = GetComponent<Parasite>();
        if (parasite != null)
        {
            parasite.Possess(Npc); 
        }

        // Reset Npc momentum 
        Rigidbody2D npcRb = Npc.GetComponent<Rigidbody2D>();
        if (npcRb != null) 
        {
            npcRb.linearVelocity = Vector2.zero;
            npcRb.angularVelocity = 0f;
        }

        // Disable parasite physics and rendering while host is controlled
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Parent parasite transform to target Npc
        transform.SetParent(Npc.transform);
        transform.localPosition = Vector3.zero;

        // Update camera focus
        if (CameraFollow.instance != null)
        {
            CameraFollow.instance.SetTarget(Npc.transform); 
        }
    }

    public void Die(){
        if (isDead) return;

        isInAir = false;
        isDead = true;
        isCharging = false;

        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        animator.SetTrigger("Die");

        if (GameManager.gameManager != null) {
            GameManager.gameManager.GameOver();
        }
    }
}
