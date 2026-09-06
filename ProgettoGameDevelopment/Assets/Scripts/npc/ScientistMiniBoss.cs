
using UnityEngine;

public class scientistMiniboss : Enemy
{
	private bool isDying = false;
	private float lastHorizontal = 0;
	private float lastVertical = -1f;
	private Movement movement;
	private Animator animator;
	[SerializeField] private GameObject ZipBomb;

	protected override void Awake() {
		base.Awake();
		this.hitPoints = 200;
		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
	}

	void Start() {
		// Parte sempre in waiting.
		// CheckForParassita() verrà controllato continuamente.
		currentState = State.waiting;
		UpdateAnimation(Vector2.zero);
	}


	void Update() {
		if (isDying) return;

		if (hitPoints <= 0) {
			Die();
			return;
		}

		switch (currentState) {
			// =========================================================
			// WAITING
			// =========================================================
			case State.waiting: {
				if (!CheckForParasite()) {
					// Nessun Parassita vicino:
					// movimento casuale.
					Vector2 direction = RandomMovement();
					movement.SetDirection(direction);
					UpdateAnimation(direction);
				} else {
					// Abbiamo rilevato il Parassita
					// oppure un corpo posseduto.
					if (parasite.currentState == Parasite.State.possessing) {
						currentState = State.escaping;
					} else {
						currentState = State.catching;
					}
				}
			break;
			}
			// =========================================================
			// CATCHING
			// =========================================================
			case State.catching: {
				// Centro dello sprite del miniboss.
				Vector2 minibossPosition = spriteRenderer.bounds.center;
				// Centro dello sprite del Parassita.
				SpriteRenderer parasiteSprite = parasite.GetComponent<SpriteRenderer>();
				Vector2 parasitePosition;
				if (parasiteSprite != null) {
					parasitePosition = parasiteSprite.bounds.center;
				} else {
					parasitePosition = parasite.transform.position;
				}

				float distance = Vector2.Distance(minibossPosition, parasitePosition);

				if (distance > 0.1f) {
					Vector2 captureDirection = GetBestDirection(parasitePosition, Vector2.zero);
					movement.SetDirection( captureDirection);
					UpdateAnimation(captureDirection);
				} else {
					movement.SetDirection(Vector2.zero);
					UpdateAnimation(Vector2.zero);
				}
				// Se il Parassita possiede qualcuno,
				// il miniboss deve scappare.
				if (parasite.currentState == Parasite.State.possessing) {
					currentState = State.escaping;
				}
			break;
			}
			// =========================================================
			// ESCAPING
			// =========================================================
			case State.escaping: {
				// Centro dello sprite del Parassita.
				Vector2 threatPosition = GetTargetPosition();
				// Calcolo la direzione di fuga.
				Vector2 fleeDirection = GetEscapeDirection(threatPosition);
				movement.SetDirection(fleeDirection);
				UpdateAnimation(fleeDirection);
				// Se il Parassita torna libero,
				// torniamo a inseguirlo.
				if (parasite.currentState == Parasite.State.free) {
					currentState = State.catching;
				}
			break;
			}
		}
	}
	// =========================================================
	// MORTE
	// =========================================================
	protected override void Die() {
		if (isDying) return;
		isDying = true;

		if (movement != null) {
			movement.speed = 0f;
			movement.SetDirection(Vector2.zero);
		}
		// Drop potenziamento
		Instantiate(ZipBomb, spriteRenderer.bounds.center, Quaternion.identity);
		Destroy(gameObject);
	}
	// =========================================================
	// COLLISIONI
	// =========================================================
	private void OnCollisionEnter2D(Collision2D collision) {
		// Il proiettile fa danno al miniboss.
		if (collision.gameObject.CompareTag("Bullet")) {
			ReceiveDamage(10);
			return;
		}
		// Collisione con il Parassita.
		Parasite collidedParasite =collision.gameObject.GetComponent<Parasite>();

		if (currentState == State.catching && collidedParasite != null) {
			PlayerJump playerJump = collidedParasite.GetComponent<PlayerJump>();

			if (playerJump != null && !playerJump.isInAir) {
				collidedParasite.Die();
			}
		}
	}
	// =========================================================
	// ANIMAZIONE
	// =========================================================
	private void UpdateAnimation(Vector2 direction) {
		if (animator == null) return;

		if (direction != Vector2.zero) {
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

