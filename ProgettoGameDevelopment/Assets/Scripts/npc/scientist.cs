using UnityEngine;

public class scientist : Nemico
{
	private bool isDying=false;
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
		up=isFree(Vector2.up);
		down=isFree(Vector2.down);
		left=isFree(Vector2.left);
		right=isFree(Vector2.right);
		
		if (HitPoints<=0) {
			Die();
			return;
		}

		switch (StatoAttuale)
		{
			case Stato.catching:
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
				if (parassita.StatoAttuale==Parassita.Stato.libero) 
					{
						this.HitPoints=0;
					}
			break;
			}
		}
	}

    protected override void Die()
    {
        if (isDying) return;
		isDying = true;

		if (movement != null) movement.speed = 0f;
		Destroy(gameObject, 1.5f);
    }

	private void OnCollisionEnter2D(Collision2D collision)
{
    // Il proiettile fa danno allo scienziato
    if (collision.gameObject.name == "bullet")
    {
        PrendiDanno(10);
        return;
    }

    // Controlliamo il parassita solo se lo scienziato
    // sta cercando di catturarlo
    if (StatoAttuale == Stato.catching &&
        collision.gameObject.name == "Parassita")
    {
        if (parassita.StatoAttuale == Parassita.Stato.possessing)
        {
            Debug.Log("Lo scienziato è stato posseduto");

            StatoAttuale = Stato.possessed;
        }
        else
        {
            Debug.Log("Parassita catturato!");

            parassita.Muori();
        }
    }
}
	private void UpdateAnimation(Vector2 direction)
{
    if (animator == null) return;

    animator.SetFloat("Horizontal", direction.x);
    animator.SetFloat("Vertical", direction.y);
    animator.SetFloat("Speed", direction.sqrMagnitude);
}
}








