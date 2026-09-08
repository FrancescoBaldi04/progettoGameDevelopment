using UnityEngine;
using UnityEngine.InputSystem;

public class Parasite : MonoBehaviour
{
    public enum State {free, possessing}
    [SerializeField] private GameObject prefabExplosion;
    public GameObject possessedBody;
    public State currentState {get; private set;}
    private bool running = false;
    private float health = 60f;
    private float possessionHealth; // Starts at 60 and decreases to 0 while possessing an NPC
    private float oneSecondTimer; 
    [SerializeField] private HealthBar healthBar;
    private float explosionRadius = 3f;
    private int explosionDamage = 60;
    [SerializeField] public float moveSpeed = 2.6f;

    private Rigidbody2D rb;
    private Animator animator;

    public Vector2 movement;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";
    private const string jump = "Jump";
    private const string resetState = "ResetState";

    private PlayerJump playerJump;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(health);
    }

    private void Start()
    {
        playerJump = GetComponent<PlayerJump>();
        currentState = State.free;
    }

    private void Update()
    {
        if (Time.timeScale == 0f) return; // If the pause menu is open, prevent the parasite from moving
        
        if (!StartScreen.isGameStarted) return;
         
        if (currentState == State.possessing) 
        {
            possessionHealth = HealthDrain(possessionHealth);
        }
        else
        {
            health = HealthDrain(health);
        }

        if (health <= 0 && playerJump != null) // Death of the parasite
        {
            playerJump.Die();
        }

        if (currentState == State.possessing && possessionHealth <= 0) // Death of the possessed body
        {
            PossessedBodyDeath();
        }

        if (currentState == State.possessing)
        {
            // =========================
            // ZIP BOMB
            // =========================

            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                ZipBombExplosion();
            }
           
            if (possessedBody != null && possessedBody.GetComponent<Guard>() != null && Keyboard.current != null && 
                Keyboard.current.fKey.wasPressedThisFrame)
           {   
                Enemy enemy = possessedBody.GetComponent<Guard>();
                if (enemy != null)
                {
                    enemy.Shoot(true);
                }
            }

            return;
        }
            // =========================
            // RUN
            // =========================

        if (Keyboard.current != null && Keyboard.current.cKey.wasPressedThisFrame)
        {
            Run();
        }

        if (playerJump != null && (playerJump.isInAir || playerJump.isDead))
        {
            return;
        }

        movement = InputManager.movement;

        animator.SetFloat(horizontal, movement.x);
        animator.SetFloat(vertical, movement.y);

        if (movement != Vector2.zero)
        {
            animator.SetFloat(lastHorizontal, movement.x);
            animator.SetFloat(lastVertical, movement.y);
        }

        if (playerJump != null && playerJump.isCharging)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = moveSpeed * movement;
        }      
    }

    // =====================================================
    // POSSESS
    // =====================================================

    public void Possess(GameObject body)
    {
        possessedBody = body;
        currentState = State.possessing;

        Enemy enemy = body.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.currentState = Enemy.State.possessed; 
        }

        possessionHealth = 60f;
        oneSecondTimer = 0f;
        healthBar.SetMaxHealth(possessionHealth);
    }


    public void TakeDamage(int danno)
    {
        if (currentState == State.possessing)
        {
            possessionHealth -= danno;

            if (possessionHealth < 0) possessionHealth = 0; // If the health points are below zero, set them to zero

            healthBar.SetHealth(possessionHealth);

            if (possessionHealth <= 0)
            {
                PossessedBodyDeath();
            }
        }
    }

    private float HealthDrain(float health)
    {
        oneSecondTimer += Time.deltaTime; 

        if (oneSecondTimer >= 1f) 
        {
            oneSecondTimer -= 1f;
            
            if (GameManager.gameManager.hasTrojanHorse) // Take less damage if the player has the Trojan Horse power-up
            {
                health -= 1f;
            }
            else
            {
                health -= 2f;
            }
        
            if (health < 0) health = 0;

            healthBar.SetHealth(health);
        }
    
        return health;
    }

    public void PossessedBodyDeath()
    {
        if (possessedBody != null)
        {
            Enemy enemy = possessedBody.GetComponent<Enemy>();
            possessedBody = null; // sgancio il corpo posseduto
            transform.SetParent(null);

            if (enemy != null)
            {
                enemy.ReceiveDamage(9999); // danno fatale, non posso modificare direttamente gli hitPoints, altrimenti potrei fare un metodo dedicato chiamato Uccidi che imposta gli hp a 0 ma è la stessa cosa sostanzialmente
            }
        }

        
        ReleaseParasite();
    }

    public void ReleaseParasite()
    {
        transform.SetParent(null);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        rb.simulated = true;

        currentState = State.free;
         if(running){
            Run();
        }
        health = 60f;

        animator.ResetTrigger(jump);  // ripristina il trigger del salto
        animator.SetTrigger(resetState); // segnala all'animator che il parassita deve tornare alla sua animazione standard
        
        if (CameraFollow.instance != null)
        {
            CameraFollow.instance.SetTarget(transform);
        }
    }

    // =====================================================
    // ZIP BOMB
    // =====================================================

    public void ZipBombExplosion()
    {  
        if (currentState != State.possessing)
        {
         
            return;
        }
 
        Vector3 position = GetPossessedBodyPosition();
        GameObject bodyToDestroy = possessedBody;

        possessedBody = null;
        healthBar.SetHealth(0);

        ReleaseParasite();

        // Distrugge il corpo sacrificato
        Destroy(bodyToDestroy);
        if (GameManager.gameManager.hasZipBomb)
        {
            GameObject explosion = Instantiate(prefabExplosion, position,Quaternion.identity);

            Animator explosionAnimator = explosion.GetComponent<Animator>();
            explosionAnimator.Play("Explosion");

            Collider2D[] hits = Physics2D.OverlapCircleAll(position, explosionRadius);

            foreach (Collider2D c in hits)
            {
                // Ignora il Parassita
                if (c.GetComponent<Parasite>() != null)
                {
                    continue;
                }

                Enemy enemy = c.GetComponent<Enemy>();

                if (enemy != null)
                {
                    enemy.ReceiveDamage(explosionDamage); 
                }
            }
        }
    }

    // =====================================================
    // WORM / RUN
    // =====================================================

    public void Run()
    {
        if (!GameManager.gameManager.hasWorm)
        {
            return;
        }

        if (currentState == State.free)
        {
            running = !running;

            if (running)
            {
                moveSpeed = moveSpeed*2;
            }
            else
            {
                moveSpeed = moveSpeed/2;
            }
        }
    }
    public void Die()
    {
        if (playerJump != null)
        {
            playerJump.Die();
        }
    }
    public Vector2 GetPossessedBodyPosition()
    {
        if (possessedBody == null)
            return transform.position;

        SpriteRenderer sr = possessedBody.GetComponent<SpriteRenderer>();

        if (sr != null) return sr.bounds.center;

        return possessedBody.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            TakeDamage(10);
            return;
        }
    }
    
}