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

    private float timerConsumo;

    private float raggioEsplosione = 5f;
    private int dannoEsplosione = 50;

    [SerializeField] public float moveSpeed = 1.5f;

    private Rigidbody2D rb;
    private Animator animator;

    public Vector2 movement;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";
    private const string lastHorizontal = "LastHorizontal";
    private const string lastVertical = "LastVertical";

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
        timerConsumo = 0;
    }


    private void Update()
    {
        if (!StartScreen.giocoIniziato)
        {
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
        // ZIP BOMB
        // =========================

        if (Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            EsplosioneZipBomb();
        }


        // =========================
        // CONSUMO POSSESSO
        // =========================

        if (StatoAttuale == Stato.possessing)
        {
            ConsumoPossesso();
        }


        // =========================
        // RUN
        // =========================

        if (Keyboard.current != null &&
            Keyboard.current.cKey.wasPressedThisFrame)
        {
            Run();
        }
    }


    // =====================================================
    // POSSESSO
    // =====================================================

    public void Possiedi(GameObject corpo)
    {
        corpoPosseduto = corpo;

        statoAttuale = Stato.possessing;

        timerConsumo = 60;
    }


    public void SubisciDanno(int danno)
    {
        if (StatoAttuale == Stato.possessing)
        {
            timerConsumo -= danno;

            if (timerConsumo <= 0)
            {
                // eventualmente morte del posseduto
            }
        }
    }


    private void ConsumoPossesso()
    {
        timerConsumo += Time.deltaTime;

        if (timerConsumo >= 1f)
        {
            if (GameManager.gameManager.hasTrojanHorse)
            {
                timerConsumo -= 1;
            }
            else
            {
                timerConsumo -= 2;
            }
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


        // Distrugge il corpo sacrificato
        Destroy(corpoPosseduto);

        corpoPosseduto = null;

        statoAttuale = Stato.libero;
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


        if (running)
        {
            moveSpeed += 2f;
        }
        else
        {
            moveSpeed -= 2f;
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