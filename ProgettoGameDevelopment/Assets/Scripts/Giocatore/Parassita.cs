using UnityEngine;
using UnityEngine.InputSystem;

public class Parassita : MonoBehaviour
{
    public enum Stato
    {
        libero,
        possessing
    }
  [SerializeField] private GameObject esplosionePrefab;
    public GameObject corpoPosseduto;

    public Stato statoAttuale;

    public Stato StatoAttuale => statoAttuale;

    private bool running = false;

    private float health = 60f;
    private float vitaPossesso; // parte da 60 e scende fino a 0 durante il possesso di un npc
    private float timerSecondo; // conta fino ad un secondo in modo da diminuire la vita ogni secondo
    [SerializeField] private HealthBar healthBar;
    private float raggioEsplosione = 5f;
    private int dannoEsplosione = 60;
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
        statoAttuale = Stato.libero;
    }


    private void Update()
    {
        if (Time.timeScale == 0f) return; // se siamo nel menu di pausa non permettiamo al parassita di muoversi
        
        if (!StartScreen.giocoIniziato) return;
         
        if (statoAttuale == Stato.possessing) // consumo vita
        {
            vitaPossesso = ConsumoVita(vitaPossesso);
        }
        else
        {
            health = ConsumoVita(health);
        }

        if (health <= 0 && playerJump != null) // morte parassita
        {
            playerJump.die();
        }

        if (statoAttuale == Stato.possessing && vitaPossesso <= 0) // morte corpo posseduto
        {
            MorteCorpoPosseduto();
        }

        if (statoAttuale == Stato.possessing)
        {
            // =========================
            // ZIP BOMB
            // =========================

            if (Keyboard.current != null &&
                Keyboard.current.eKey.wasPressedThisFrame)
            {
                EsplosioneZipBomb();
            }
           
            if (corpoPosseduto != null && corpoPosseduto.GetComponent<guard>() != null && Keyboard.current != null && Keyboard.current.fKey.wasPressedThisFrame)
           {   
             Nemico nemico = corpoPosseduto.GetComponent<guard>();
           if (nemico != null)
           {
             nemico.Shoot(true);
           }
}

            return;
        }
         // =========================
            // RUN
            // =========================

            if (Keyboard.current != null &&
                Keyboard.current.cKey.wasPressedThisFrame)
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
    // POSSESSO
    // =====================================================

    public void Possiedi(GameObject corpo)
    {
        corpoPosseduto = corpo;
        statoAttuale = Stato.possessing;

        Nemico nemico = corpo.GetComponent<Nemico>();
        if (nemico != null)
        {
            nemico.StatoAttuale = Nemico.Stato.possessed; // imposto lo stato dell'npc in possessed
        }

        vitaPossesso = 60f;
        timerSecondo = 0f;
        healthBar.SetMaxHealth(vitaPossesso);
    }


    public void SubisciDanno(int danno)
    {
        if (StatoAttuale == Stato.possessing)
        {
            vitaPossesso -= danno;

            if (vitaPossesso < 0) vitaPossesso = 0; // se i punti vita sono sotto zero li porto a zero

            healthBar.SetHealth(vitaPossesso);

            if (vitaPossesso <= 0)
            {
                MorteCorpoPosseduto();
            }
        }
    }

    private float ConsumoVita(float health)
    {
        timerSecondo += Time.deltaTime; 

        if (timerSecondo >= 1f) // conto 1 secondo e resetto il timer in modo da decrementare la vita ogni secondo
        {
            timerSecondo -= 1f;
            
            if (GameManager.gameManager.hasTrojanHorse) // controllo Trojan Horse
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

    public void MorteCorpoPosseduto()
    {
        if (corpoPosseduto != null)
        {
            Nemico nemico = corpoPosseduto.GetComponent<Nemico>();
           corpoPosseduto = null; // sgancio il corpo posseduto
            transform.SetParent(null);

            if (nemico != null)
            {
                nemico.PrendiDanno(9999); // danno fatale, non posso modificare direttamente gli hitPoints, altrimenti potrei fare un metodo dedicato chiamato Uccidi che imposta gli hp a 0 ma è la stessa cosa sostanzialmente
            }
        }

        
        LiberaParassita();
    }

    public void LiberaParassita()
    {
       
        transform.SetParent(null);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        rb.simulated = true;

        statoAttuale = Stato.libero;
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

    public void EsplosioneZipBomb()
    {
       
        if (statoAttuale != Stato.possessing)
        {
         
            return;
        }
        
         GameObject corpoDaDistruggere = corpoPosseduto;
         if (!GameManager.gameManager.hasZipBomb)
        {
         

        corpoPosseduto = null;
        healthBar.SetHealth(0);

        LiberaParassita();

        // Distrugge il corpo sacrificato
        Destroy(corpoDaDistruggere);

            return;
        }


       


        Vector3 posizione =
        corpoPosseduto.transform.position;
        GameObject esplosione = Instantiate(
            esplosionePrefab,
            posizione,
            Quaternion.identity
        );

        Animator animatorEsplosione = esplosione.GetComponent<Animator>();
        animatorEsplosione.Play("Explosion");

        Collider2D[] colpiti = Physics2D.OverlapCircleAll(posizione, raggioEsplosione);

        foreach (Collider2D c in colpiti)
        {
            // Ignora il Parassita
            if (c.GetComponent<Parassita>() != null)
            {
                continue;
            }


            Nemico nemico = c.GetComponent<Nemico>();


            if (nemico != null)
            {
                nemico.PrendiDanno(dannoEsplosione);

             
            }
        }
       

        corpoPosseduto = null;
        healthBar.SetHealth(0);

        LiberaParassita();

        // Distrugge il corpo sacrificato
        Destroy(corpoDaDistruggere);

        
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

       if (statoAttuale == Stato.libero)
        {running = !running;
       
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
    public void Muori()
    {
       

        if (playerJump != null)
        {
            playerJump.die();
        }
    }
    public Vector2 GetCorpoPossedutoPosition()
    {
        if (corpoPosseduto == null)
            return transform.position;

        SpriteRenderer sr =
            corpoPosseduto.GetComponent<SpriteRenderer>();

        if (sr != null)
            return sr.bounds.center;

        return corpoPosseduto.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            SubisciDanno(10);
            return;
        }
    }
    
}