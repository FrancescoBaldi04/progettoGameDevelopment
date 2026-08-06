using NUnit.Framework;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    [SerializeField] private float maxChargeTime = 1.5f;
    [SerializeField] private float baseJumpForce = 5f;
    [SerializeField] private float maxJumpForce = 15f;

    public bool isCharging {get; private set;} = false; // variabili che dovrà leggere PlayerMovement in modo da bloccare gli altri comandi durante il volo
    public bool isInAir {get; private set;} = false;

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
        if (isInAir) return;

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

        Vector2 jumpDirection = InputManager.movement.normalized; // calcolo la direzione del salto in base all'ultimo tasto premuto

        if (jumpDirection == Vector2.zero) { // se non ho input di movimento utilizzo la direzione in cui guarda il parassita
            
            float lastX = animator.GetFloat("LastHorizontal");
            float lastY = animator.GetFloat("LastVertical");
            jumpDirection = new Vector2(lastX, lastY).normalized;

            if (jumpDirection == Vector2.zero) jumpDirection = Vector2.down; // in caso abbia appena avviato il gioco e non mi sia mai mosso prima
        }

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
        // logica di possessione Npc
    }

    private void die()
    {
        // logica di morte e game over
    }
}
