using NUnit.Framework;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float maxChargeTime = 1.5f;
    [SerializeField] private float baseJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 15f;

    public bool isCharging {get; private set;} = false; // variabili che dovrà leggere PlayerMovement in modo da bloccare gli altri comandi durante il volo
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
        if (isInAir || isDead) return;

        chargeInput();
    }

    private void chargeInput(){
        if (InputManager.chargeStarted){ // tasto appena premuto
            isCharging = true;
            chargeTimer = 0f;
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isCharging", true);
        }

        if (InputManager.chargeHeld && isCharging){ // il tasto non è ancora stato rilasciato
            chargeTimer += Time.deltaTime;
            float chargePercent = Mathf.Clamp01(chargeTimer / maxChargeTime); // calcolo la percentuale di carica

            if (chargePercent < 0.33f) {
                currentChargeState = 1;
            }else if (chargePercent < 0.66f) {
                currentChargeState = 2;
            }else{
                currentChargeState = 3;   
            }

            animator.SetInteger("chargeState", currentChargeState);
        }


        if (InputManager.chargeReleased && isCharging){
            jump();
        }
    }

    private void jump(){
        isCharging = false;
        isInAir = true;

        float lastX = animator.GetFloat("LastHorizontal");
        float lastY = animator.GetFloat("LastVertical");
        Vector2 jumpDirection = new Vector2(lastX, lastY).normalized; // salta nella direzione dell'ultimo tasto premuto

        if (jumpDirection == Vector2.zero) jumpDirection = Vector2.down; // in caso abbia appena avviato il gioco e non mi sia mai mosso prima

        float finalForce = Mathf.Lerp(baseJumpForce, maxJumpForce, (float)currentChargeState / 3f); // calcolo la forza da applicare in modo proporzionale alla carica, Lerp interpola linearmente tra baseJumpForce e maxJumpForce attraverso il valore ottenuto a partire dalla carica
        rb.AddForce(jumpDirection * finalForce, ForceMode2D.Impulse); // impulso verso la direzione del salto

        animator.SetBool("isCharging", false);
        animator.SetTrigger("Jump");
        currentChargeState = 0;
    }

    private void OnCollisionEnter2D (Collision2D collision) {
        if (isInAir){
            if (collision.gameObject.CompareTag("Npc")){ // ricordarsi di assegnare il tag Npc ai prefab degli npc
                possessNpc(collision.gameObject);
            }else{
                die();   
            }
        }
    }

    private void possessNpc(GameObject Npc){
        isInAir = false;
        
        Parassita parassita = GetComponent<Parassita>();
        if (parassita != null)
        {
            parassita.Possiedi(Npc);
        }

        Rigidbody2D npcRb = Npc.GetComponent<Rigidbody2D>();
        if (npcRb != null)
        {
            npcRb.linearVelocity = Vector2.zero;
            npcRb.angularVelocity = 0f;
        }

        // Disattivo lo sprite del parassita e la sua fisica
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.simulated = false;

        // Collego il parassita all'Npc per farlo muovere insieme a lui
        transform.SetParent(Npc.transform);
        transform.localPosition = Vector3.zero;

        if (CameraFollow.instance != null)
        {
            CameraFollow.instance.SetTarget(Npc.transform); // sposto la telecamera su npc
        }
    }

    public void die(){
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
