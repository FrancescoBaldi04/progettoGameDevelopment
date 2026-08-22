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
    private GameObject corpoPosseduto;

    private Stato statoAttuale;

    public Stato StatoAttuale => statoAttuale;

    private bool running = false;

    private float vitaPossesso; // parte da 60 e scende fino a 0 durante il possesso di un npc
    private float timerSecondo; // conta fino ad un secondo in modo da diminuire la vita ogni secondo

    private float raggioEsplosione = 5f;
    private int dannoEsplosione = 50;
    private float normalSpeed;
    

    [SerializeField] public float moveSpeed = 1.5f;

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
    }


    private void Start()
    {
        playerJump = GetComponent<PlayerJump>();
       

        statoAttuale = Stato.libero;
    }


    private void Update()
    {
        if (!StartScreen.giocoIniziato) return;

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
         // =========================
        // RUN
        // =========================

        if (Keyboard.current != null &&
            Keyboard.current.cKey.wasPressedThisFrame)
        {
            Run();
        }
            // gestione eventuali input aggiuntivi da inserire qui, credo
            ConsumoPossesso();
            return;
        }

        if (playerJump != null &&
            (playerJump.isInAir || playerJump.isDead))
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


        if (playerJump != null &&
            playerJump.isCharging)
        {
            rb.linearVelocity = Vector2.zero;
        }
        else
        {
            rb.linearVelocity = moveSpeed * movement;
        }


       


        // =========================
        // CONSUMO POSSESSO
        // =========================

        if (StatoAttuale == Stato.possessing)
        {
            ConsumoPossesso();
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
    }


    public void SubisciDanno(int danno)
    {
        if (StatoAttuale == Stato.possessing)
        {
            vitaPossesso -= danno;

            if (vitaPossesso <= 0)
            {
                MorteCorpoPosseduto();
            }
        }
    }

    private void ConsumoPossesso()
    {
        timerSecondo += Time.deltaTime;

        if (timerSecondo >= 1f)
        {
            timerSecondo -= 1f;
            
            if (GameManager.gameManager.hasTrojanHorse)
            {
                vitaPossesso -= 1f;
            }
            else
            {
                vitaPossesso -= 2f;
            }
        
            if (vitaPossesso <= 0)
            {
                MorteCorpoPosseduto();
            }
        }
    }

    private void MorteCorpoPosseduto()
    {
        if (corpoPosseduto != null)
        {
            Nemico nemico = corpoPosseduto.GetComponent<Nemico>();

            if (nemico != null)
            {
                nemico.PrendiDanno(9999); // danno fatale, non posso modificare direttamente gli hitPoints, altrimenti potrei fare un metodo dedicato chiamato Uccidi che imposta gli hp a 0 ma è la stessa cosa sostanzialmente
            }
        }

        corpoPosseduto = null; // sgancio il corpo posseduto
        LiberaParassita();
    }

    public void LiberaParassita()
    {
        transform.SetParent(null);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;

        rb.simulated = true;

        statoAttuale = Stato.libero;

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
        if (!GameManager.gameManager.hasZipBomb)
        {
            Debug.Log("Zip Bomb non sbloccata");
            return;
        }


        if (statoAttuale != Stato.possessing)
        {
            Debug.Log("Zip Bomb non disponibile");
            return;
        }


        Debug.Log("BOOM! Zip Bomb esplosa");


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

                Debug.Log(
                    "Danno Zip Bomb a " +
                    nemico.name
                );
            }
        }
        GameObject corpoDaDistruggere = corpoPosseduto;

    corpoPosseduto = null;

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


        running = !running;


       if (statoAttuale == Stato.possessing)
    {
        if (corpoPosseduto == null)
        {
            return;
        }

        Movement movement = corpoPosseduto.GetComponent<Movement>();

        if (movement == null)
        {
            Debug.LogWarning("Il corpo posseduto non ha il componente Movement!");
            return;
        }

        if (running)
        {
            movement.speed += 0.1f;
        }
        else
        {
            movement.speed -= 0.1f;
        }
    }
    }
    public void Muori()
    {
        Debug.Log("Il parassita è morto!");

        if (playerJump != null)
        {
            playerJump.die();
        }
    }
}