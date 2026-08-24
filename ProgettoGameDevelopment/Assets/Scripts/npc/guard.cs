using UnityEngine;

public class guard : Nemico
{
    public float targetDistance = 3.0f;

    private float timer = 1.0f;
    private bool isDying = false;

    private Movement movement;
    private Animator animator;

    private int obstacleLayerMask;

    // Ultima direzione in cui la guardia si è mossa.
    // Serve quando è ferma per scegliere Idle e Shoot.
    private float lastHorizontal = 0f;
    private float lastVertical = -1f;


    protected override void Awake()
    {
        base.Awake();

        movement = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        

        obstacleLayerMask = LayerMask.GetMask("ground");
    }


    void Start()
    {
        if (parassita.StatoAttuale == Parassita.Stato.possessing)
        {
            StatoAttuale = Stato.positioning;
        }
        else
        {
            StatoAttuale = Stato.escaping;
        }

        // La guardia inizialmente guarda verso il basso.
        UpdateAnimation(Vector2.zero);
    }


    void Update()
    {
        if (isDying)
            return;

        if (HitPoints <= 0)
        {
            Die();
            return;
        }


        switch (StatoAttuale)
        {
            // =====================================================
            // ESCAPING
            // =====================================================

            case Stato.escaping:
            {
                Vector2 versoDiFuga =
                    ((Vector2)transform.position -
                     (Vector2)parassita.transform.position).normalized;

                movement.SetDirection(versoDiFuga);
                UpdateAnimation(versoDiFuga);


                if (parassita.StatoAttuale ==
                    Parassita.Stato.possessing)
                {
                    StatoAttuale = Stato.positioning;
                }

                break;
            }


            // =====================================================
            // POSITIONING
            // =====================================================

            case Stato.positioning:
            {
                if (parassita.StatoAttuale ==
                    Parassita.Stato.libero)
                {
                    StatoAttuale = Stato.escaping;
                    break;
                }


                if (movement != null && parassita != null)
                {
                    Vector2 myPosition = transform.position;
                    Vector2 parassitaPosition = parassita.transform.position;

                    float distance =
                        Vector2.Distance(myPosition, parassitaPosition);

                    Vector2 directionToParassita =
                        (parassitaPosition - myPosition).normalized;


                    RaycastHit2D hit =
                        Physics2D.Raycast(
                            myPosition,
                            directionToParassita,
                            distance,
                            obstacleLayerMask
                        );


                    // Non è ancora nella posizione giusta:
                    // continua a muoversi verso il parassita.
                    if (hit.collider != null ||
                        distance > targetDistance + 0.3f)
                    {
                        movement.SetDirection(directionToParassita);
                        UpdateAnimation(directionToParassita);
                    }
                    else
                    {
                        // Ha raggiunto la posizione da cui può sparare.
                        movement.SetDirection(Vector2.zero);
                        UpdateAnimation(Vector2.zero);

                        StatoAttuale = Stato.shooting;
                        timer = 1.0f;
                    }
                }

                break;
            }


            // =====================================================
            // SHOOTING
            // =====================================================

            case Stato.shooting:
{
    movement.SetDirection(Vector2.zero);

    Vector2 origine;

    if (spriteRenderer != null)
        origine = spriteRenderer.bounds.center;
    else
        origine = transform.position;

    Vector2 targetPosition = GetTargetPosition();

    Vector2 directionToTarget =
        (targetPosition - origine).normalized;

    // Aggiorna continuamente la direzione
    // verso il corpo posseduto
    if (directionToTarget != Vector2.zero)
    {
        lastHorizontal = directionToTarget.x;
        lastVertical = directionToTarget.y;

        animator.SetFloat("LastHorizontal", lastHorizontal);
        animator.SetFloat("LastVertical", lastVertical);
    }

    timer -= Time.deltaTime;

    float distance =
        Vector2.Distance(origine, targetPosition);

    RaycastHit2D hit =
        Physics2D.Raycast(
            origine,
            directionToTarget,
            distance,
            obstacleLayerMask
        );

    if (hit.collider != null || distance > targetDistance + 0.3f)
{
    StatoAttuale = Stato.positioning;
    timer = 1.0f;
}
    else if (timer <= 0)
    {
        animator.SetTrigger("Shooting");

        Shoot();

        timer = 1.0f;
    }
    if(parassita.StatoAttuale==Parassita.Stato.libero){
StatoAttuale = Stato.escaping;
    }
    break;
}

            // =====================================================
            // POSSESSED
            // =====================================================

            case Stato.possessed:
            {
                if (parassita.StatoAttuale ==
                    Parassita.Stato.libero)
                {
                    HitPoints = 0;
                    break;
                }


                Vector2 inputGiocatore = InputManager.movement;

                movement.SetDirection(inputGiocatore);

                UpdateAnimation(inputGiocatore);

                break;
            }
        }
    }
   


    // =========================================================
    // ANIMAZIONI
    // =========================================================

    private void UpdateAnimation(Vector2 direction)
    {
        if (animator == null)
            return;


        // Memorizzo l'ultima direzione SOLO quando la guardia
        // si sta effettivamente muovendo.
        if (direction != Vector2.zero)
        {
            lastHorizontal = direction.x;
            lastVertical = direction.y;
        }


        // Direzione attuale.
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);

        // Velocità attuale.
        animator.SetFloat("Speed", direction.magnitude);

        // Ultima direzione valida.
        animator.SetFloat("LastHorizontal", lastHorizontal);
        animator.SetFloat("LastVertical", lastVertical);
    }


    // =========================================================
    // MORTE
    // =========================================================

    protected override void Die()
    {
        if (isDying)
            return;

        isDying = true;

        if (movement != null)
        {
            movement.speed = 0f;
            movement.SetDirection(Vector2.zero);
        }

        Destroy(gameObject, 1.5f);
    }


    
}