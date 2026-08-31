
using UnityEngine;

public class guard : Nemico
{
	public float targetDistance = 3.0f;
	private float timer = 1.0f;
	private bool isDying = false;

	private Movement movement;
	private Animator animator;

	private int obstacleLayerMask;

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
		// Tutte le guardie iniziano in waiting.
		// CheckForParassita() deciderà poi cosa fare.
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
					// Nessun Parassita nelle vicinanze:
					// movimento casuale.

					Vector2 direction = RandomMovement();

					movement.SetDirection(direction);
					UpdateAnimation(direction);
				}
				else
				{
					// Abbiamo rilevato il Parassita
					// oppure un NPC posseduto.

					if (parassita.StatoAttuale ==
						Parassita.Stato.possessing)
					{
						StatoAttuale = Stato.positioning;
					}
					else
					{
						StatoAttuale = Stato.escaping;
					}
				}

				break;
			}


			// =========================================================
			// ESCAPING
			// =========================================================

			case Stato.escaping:
			{
				// Centro dello sprite della guardia.
				Vector2 guardPosition =
					spriteRenderer.bounds.center;

				// Centro dello sprite del Parassita.
				Vector2 parassitaPosition =
					parassita.GetComponent<SpriteRenderer>()
					.bounds.center;


				Vector2 versoDiFuga =
					-GetBestDirection(
						parassitaPosition,
						Vector2.zero
					);


				movement.SetDirection(versoDiFuga);
				UpdateAnimation(versoDiFuga);


				if (parassita.StatoAttuale ==
					Parassita.Stato.possessing)
				{
					StatoAttuale = Stato.positioning;
				}

				break;
			}


			// =========================================================
			// POSITIONING
			// =========================================================

			case Stato.positioning:
			{
				if (parassita.StatoAttuale ==
					Parassita.Stato.libero)
				{
					StatoAttuale = Stato.escaping;
					break;
				}


				if (movement != null &&
					parassita != null)
				{
					// Centro dello sprite della guardia.
					Vector2 myPosition =
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


					float distance =
						Vector2.Distance(
							myPosition,
							parassitaPosition
						);


					Vector2 directionToParassita =
						(parassitaPosition - myPosition)
						.normalized;


					RaycastHit2D hit =
						Physics2D.Raycast(
							myPosition,
							directionToParassita,
							distance,
							obstacleLayerMask
						);


					if (hit.collider != null ||
						distance > targetDistance + 0.3f)
					{
						movement.SetDirection(
							directionToParassita
						);

						UpdateAnimation(
							directionToParassita
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

						StatoAttuale =
							Stato.shooting;
					}
				}

				break;
			}


			// =========================================================
			// SHOOTING
			// =========================================================

			case Stato.shooting:
			{
				movement.SetDirection(Vector2.zero);


				// Centro dello sprite della guardia.
				Vector2 origine =
					spriteRenderer.bounds.center;


				// Posizione del bersaglio.
				Vector2 targetPosition =
					GetTargetPosition();


				Vector2 directionToTarget =
					(targetPosition - origine).normalized;


				if (directionToTarget != Vector2.zero)
				{
					lastHorizontal =
						directionToTarget.x;

					lastVertical =
						directionToTarget.y;


					animator.SetFloat(
						"LastHorizontal",
						lastHorizontal
					);

					animator.SetFloat(
						"LastVertical",
						lastVertical
					);
				}


				timer -= Time.deltaTime;


				float distance =
					Vector2.Distance(
						origine,
						targetPosition
					);


				RaycastHit2D hit =
					Physics2D.Raycast(
						origine,
						directionToTarget,
						distance,
						obstacleLayerMask
					);


				if (hit.collider != null ||
					distance > targetDistance + 0.3f)
				{
					StatoAttuale =
						Stato.positioning;

					timer = 1.0f;
				}
				else if (timer <= 0)
				{
					animator.SetTrigger("Shooting");

					Shoot(false);

					timer = 1.0f;
				}


				if (parassita.StatoAttuale ==
					Parassita.Stato.libero)
				{
					StatoAttuale =
						Stato.escaping;
				}

				break;
			}


			// =========================================================
			// POSSESSED
			// =========================================================

			case Stato.possessed:
			{
				if (parassita.StatoAttuale ==
					Parassita.Stato.libero)
				{
					HitPoints = 0;
					break;
				}


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
	// ANIMAZIONI
	// =========================================================

	private void UpdateAnimation(Vector2 direction)
	{
		if (animator == null)
			return;


		// Memorizzo l'ultima direzione SOLO
		// quando la guardia si sta muovendo.

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
			direction.magnitude
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


		Destroy(gameObject, 1.5f);
	}


	// =========================================================
	// COLLISIONI
	// =========================================================

	private void OnCollisionEnter2D(
		Collision2D collision)
	{
		// Il proiettile fa danno alla guardia.

		if (collision.gameObject.CompareTag("Bullet"))
		{
			if (parassita.corpoPosseduto != null &&
				parassita.corpoPosseduto
				.GetComponent<guard>() != null)
			{
				parassita.SubisciDanno(10);
				return;
			}
			else
			{
				PrendiDanno(10);
				return;
			}
		}
	}
}
