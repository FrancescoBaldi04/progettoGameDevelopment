
using UnityEngine;

public class scientistMiniboss : Nemico
{
	private bool isDying = false;

	private float lastHorizontal = 0;
	private float lastVertical = -1f;

	private Movement movement;
	private Animator animator;

	[SerializeField] private GameObject ZipBomb;


	protected override void Awake()
	{
		base.Awake();

		this.HitPoints = 200;

		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
	}


	void Start()
	{
		// Parte sempre in waiting.
		// CheckForParassita() verrà controllato continuamente.
		StatoAttuale = Stato.waiting;

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

			// =========================================================
			// WAITING
			// =========================================================

			case Stato.waiting:
			{
				if (!CheckForParassita())
				{
					// Nessun Parassita vicino:
					// movimento casuale.

					Vector2 direction = RandomMovement();

					movement.SetDirection(direction);
					UpdateAnimation(direction);
				}
				else
				{
					// Abbiamo rilevato il Parassita
					// oppure un corpo posseduto.

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


			// =========================================================
			// CATCHING
			// =========================================================

			case Stato.catching:
			{
				// Centro dello sprite del miniboss.
				Vector2 minibossPosition =
					spriteRenderer.bounds.center;


				// Centro dello sprite del Parassita.
				SpriteRenderer parassitaSprite =
					parassita.GetComponent<SpriteRenderer>();


				Vector2 parassitaPosition;

				if (parassitaSprite != null)
				{
					parassitaPosition =
						parassitaSprite.bounds.center;
				}
				else
				{
					parassitaPosition =
						parassita.transform.position;
				}


				float distanza =
					Vector2.Distance(
						minibossPosition,
						parassitaPosition
					);


				if (distanza > 0.1f)
				{
					Vector2 VersoDiCattura =
						GetBestDirection(
							parassitaPosition,
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


				// Se il Parassita possiede qualcuno,
				// il miniboss deve scappare.

				if (parassita.StatoAttuale ==
					Parassita.Stato.possessing)
				{
					StatoAttuale = Stato.escaping;
				}

				break;
			}


			// =========================================================
			// ESCAPING
			// =========================================================

			case Stato.escaping:
			{
				// Centro dello sprite del Parassita.
				Vector2 posizionePericolo =
					GetTargetPosition();


				// Calcolo la direzione di fuga.
				Vector2 VersoDiFuga =
					GetEscapeDirection(
						posizionePericolo
					);


				movement.SetDirection(
					VersoDiFuga
				);

				UpdateAnimation(
					VersoDiFuga
				);


				// Se il Parassita torna libero,
				// torniamo a inseguirlo.

				if (parassita.StatoAttuale ==
					Parassita.Stato.libero)
				{
					StatoAttuale =
						Stato.catching;
				}

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
		{
			movement.speed = 0f;
			movement.SetDirection(
				Vector2.zero
			);
		}


		// Drop potenziamento
		Instantiate(
			ZipBomb,
			spriteRenderer.bounds.center,
			Quaternion.identity
		);


		Destroy(gameObject, 1f);
	}


	// =========================================================
	// COLLISIONI
	// =========================================================

	private void OnCollisionEnter2D(
		Collision2D collision)
	{
		// Il proiettile fa danno al miniboss.

		if (collision.gameObject.CompareTag("Bullet"))
		{
			PrendiDanno(10);
			return;
		}


		// Collisione con il Parassita.

		Parassita parassitaScontrato =
			collision.gameObject
			.GetComponent<Parassita>();


		if (StatoAttuale == Stato.catching &&
			parassitaScontrato != null)
		{
			PlayerJump playerJump =
				parassitaScontrato
				.GetComponent<PlayerJump>();


			if (playerJump != null &&
				!playerJump.isInAir)
			{
				Debug.Log(
					"Parassita catturato!"
				);

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


		animator.SetFloat(
			"Horizontal",
			direction.x
		);

		animator.SetFloat(
			"Vertical",
			direction.y
		);

		animator.SetFloat(
			"Speed",
			direction.sqrMagnitude
		);

		animator.SetFloat(
			"LastHorizontal",
			lastHorizontal
		);

		animator.SetFloat(
			"LastVertical",
			lastVertical
		);
	}
}

