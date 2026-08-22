using UnityEngine;

public class scientist : Nemico
{
	private bool isDying=false;
    private float lastHorizontal = 0;
    private float lastVertical = -1f;
	private Movement movement;
    private Animator animator;
    protected override void Awake()
    {
        base.Awake();
		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
    }

	void Start() {		
		if (parassita.StatoAttuale==Parassita.Stato.possessing) {	
			StatoAttuale=Stato.escaping;
		} else {
			StatoAttuale=Stato.catching;
		}
	}

	void Update() {
		if (isDying) return;
				
		if (HitPoints<=0) {
			Die();
			return;
		}

		switch (StatoAttuale)
		{
			case Stato.catching:  // parentesi graffe creano uno scope locale quindi le variabili dichiarate all'interno di questo blocco saranno visibili solo in questo case e non anche negli altri come succederebbe se non le usassi
            {
                float distanza = Vector2.Distance(
                    transform.position,
                    parassita.transform.position
                );

                if (distanza > 0.1f)
                {
                    Vector2 VersoDiCattura = GetBestDirection(
                        parassita.transform.position,
                        Vector2.zero
                    );

                    movement.SetDirection(VersoDiCattura);
                    UpdateAnimation(VersoDiCattura);
                }
                else
                {
                    movement.SetDirection(Vector2.zero);
                    UpdateAnimation(Vector2.zero);
                }

                if (parassita.StatoAttuale == Parassita.Stato.possessing)
                {
                    StatoAttuale = Stato.escaping;
                }

                break;
            }
		
			case Stato.escaping: {
				Vector2 VersoDiFuga= -GetBestDirection(parassita.transform.position, Vector2.zero);
				movement.SetDirection(VersoDiFuga);
				if (parassita.StatoAttuale==Parassita.Stato.libero) {
					this.StatoAttuale=Stato.catching;
				}
			break;
			}
		
			case Stato.possessed: {
                if (parassita.StatoAttuale == Parassita.Stato.libero) 
                {
                    this.HitPoints = 0;
                    break;
                }
 
                // Leggo l'input da InputManager e lo passo al Movement dell'Npc
                Vector2 inputGiocatore = InputManager.movement;
                movement.SetDirection(inputGiocatore);
                UpdateAnimation(inputGiocatore);

                break;
			}
		}
	}

    protected override void Die()
    {
        if (isDying) return;
		isDying = true;

		if (movement != null) movement.speed = 0f;
		Destroy(gameObject, 1f);
    }

	private void OnCollisionEnter2D(Collision2D collision)
    {
        // Il proiettile fa danno allo scienziato
        if (collision.gameObject.CompareTag("Bullet"))
        {
            PrendiDanno(10);
            return;
        }

        // Quando Parassita e Scienziato entrano in collisione vengano automaticamente chiamati entrambi i metodi OnCollisionEnter quindi devo fare in modo che il parassita venga ucciso dallo scienziato solo se il parassita non è in aria e quindi non ha effettuato un salto
        Parassita parassitaScontrato = collision.gameObject.GetComponent<Parassita>();
    
        if (StatoAttuale == Stato.catching && parassitaScontrato != null)
        {
            PlayerJump playerJump = parassitaScontrato.GetComponent<PlayerJump>();
            
            if (playerJump != null && !playerJump.isInAir)
            {
                Debug.Log("Parassita catturato!");
                parassitaScontrato.Muori();
            }
        }
    }
    private void UpdateAnimation(Vector2 direction)
    {
        if (animator == null) return;

        if (direction != Vector2.zero) // se si sta muovendo memorizzo l'ultima direzione 
        {
            lastHorizontal = direction.x;
            lastVertical = direction.y;
        }

        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);
        animator.SetFloat("LastHorizontal", lastHorizontal);
        animator.SetFloat("LastVertical", lastVertical);
    }
}








