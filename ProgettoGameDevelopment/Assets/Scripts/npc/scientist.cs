
using UnityEngine;

public class scientist : Nemico
{
	private bool isDying = false;
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


	void Start()
	{
		// Parte sempre in waiting.
		// Sarà CheckForParassita() a determinare
		// successivamente se deve catturare o scappare.
		StatoAttuale = Stato.waiting;
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
			// =================================================
			// WAITING
			// =================================================

			case Stato.waiting:
			{
				if (!CheckForParassita())
				{
					

					Vector2 direction = RandomMovement();

					movement.SetDirection(direction);
					UpdateAnimation(direction);
				}
				else
				{
					

					if (parassita.StatoAttuale ==
						Parassita.Stato.possessing)
					{
						StatoAttuale = Stato.escaping;
					}
					else
					{
						StatoAttuale = Stato.catching;
					}
				}

				break;
			}


			// =================================================
			// CATCHING
			// =================================================

			case Stato.catching:
			 
			{
				if (!CheckForParassita())
		{
				StatoAttuale = Stato.waiting;
				movement.SetDirection(Vector2.zero);
				UpdateAnimation(Vector2.zero);
				break;
		}
				// Centro dello sprite dello scienziato
				Vector2 scientistPosition =
					spriteRenderer.bounds.center;


				// Centro dello sprite del Parassita
				Vector2 parasitePosition =
					parassita.GetComponent<SpriteRenderer>()
					.bounds.center;


				float distanza =
					Vector2.Distance(
						scientistPosition,
						parasitePosition
					);


				if (distanza > 0.1f)
				{
					Vector2 VersoDiCattura =
						GetBestDirection(
							parasitePosition,
							Vector2.zero
						);


					movement.SetDirection(
						VersoDiCattura
					);

					UpdateAnimation(
						VersoDiCattura
					);
				}
				else
				{
					movement.SetDirection(
						Vector2.zero
					);

					UpdateAnimation(
						Vector2.zero
					);
				}


				// Se il Parassita entra nel corpo di un NPC,
				// lo scienziato deve iniziare a scappare.

				if (parassita.StatoAttuale ==
					Parassita.Stato.possessing)
				{
					StatoAttuale = Stato.escaping;
				}

				break;
			}


			// =================================================
			// ESCAPING
			// =================================================

			case Stato.escaping:
			{
				 if (parassita.StatoAttuale == Parassita.Stato.libero)
				{
					StatoAttuale = Stato.waiting;
					movement.SetDirection(Vector2.zero);
					UpdateAnimation(Vector2.zero);
					break;
				}
				
				Vector2 posizionePericolo = GetTargetPosition();
				Vector2 VersoDiFuga = GetEscapeDirection(posizionePericolo);
				
				movement.SetDirection(VersoDiFuga);
				UpdateAnimation(VersoDiFuga);

				// Se il Parassita torna libero,
				// lo scienziato torna a cercarlo.

				if (parassita.StatoAttuale ==
					Parassita.Stato.libero)
				{
					StatoAttuale = Stato.catching;
				}

				break;
			}


			// =================================================
			// POSSESSED
			// =================================================

			case Stato.possessed:
			{
				if (parassita.StatoAttuale ==
					Parassita.Stato.libero)
				{
					this.HitPoints = 0;
					break;
				}


				// Input del giocatore
				Vector2 inputGiocatore =
					InputManager.movement;


				movement.SetDirection(
					inputGiocatore
				);

				UpdateAnimation(
					inputGiocatore
				);

				break;
			}
		}
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
			movement.speed = 0f;


		Destroy(gameObject, 1f);
	}


	// =========================================================
	// COLLISIONI
	// =========================================================

	private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Bullet"))
    {
        // Controllo se QUESTO corpo è quello posseduto dal parassita
        if (parassita.corpoPosseduto == gameObject)
        {
            parassita.SubisciDanno(10);
        }
        else
        {
            PrendiDanno(10);
        }

        return;
    }



		// Collisione con il Parassita

		Parassita parassitaScontrato = collision.gameObject.GetComponent<Parassita>();


		if (StatoAttuale == Stato.catching && parassitaScontrato != null)
		{
			PlayerJump playerJump = parassitaScontrato.GetComponent<PlayerJump>();


			if (playerJump != null && !playerJump.isInAir)
			{

				parassitaScontrato.Muori();
			}
		}
	}


	// =========================================================
	// ANIMAZIONE
	// =========================================================

	private void UpdateAnimation(
		Vector2 direction)
	{
		if (animator == null)
			return;


		if (direction != Vector2.zero)
		{
			lastHorizontal = direction.x;
			lastVertical = direction.y;
		}


		animator.SetFloat("Horizontal",direction.x);

		animator.SetFloat("Vertical",direction.y);

		animator.SetFloat("Speed",direction.sqrMagnitude);

		animator.SetFloat("LastHorizontal",lastHorizontal);

		animator.SetFloat("LastVertical",lastVertical);
	}
}

