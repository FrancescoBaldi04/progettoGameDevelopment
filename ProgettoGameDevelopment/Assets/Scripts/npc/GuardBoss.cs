using UnityEngine;

public class GuardBoss : Enemy
{
	public float targetDistance = 7.5f;
	private float timer = 1.0f;
	private bool isDying = false;
	private Movement movement;
	private Animator animator;
	private int obstacleLayerMask;
	private float lastHorizontal = 0f;
	private float lastVertical = -1f;

	protected override void Awake() {
		base.Awake();
		this.hitPoints=500;
		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
		obstacleLayerMask = LayerMask.GetMask("ground");
	}

	void Start() {
		if (parasite.currentState == Parasite.State.possessing)
		{
			currentState = State.positioning;
		} else {
			currentState = State.waiting;
		}
		UpdateAnimation(Vector2.zero);
	}

	void Update() {
		if (isDying) return;
		if (hitPoints <= 0)
		{
			Die();
			return;
		}


		switch (currentState) {
		
		case State.waiting: {
			movement.SetDirection(Vector2.zero);
			if (parasite.currentState ==Parasite.State.possessing) {
				currentState = State.positioning;
			}
		break;
		}
			
		case State.positioning: { 
				if (parasite.currentState == Parasite.State.free) {
					currentState = State.waiting;
					break;
				}
				
				if (movement != null && parasite != null) {
					// Centro dello sprite della guardia.
					Vector2 myPosition = spriteRenderer.bounds.center;
					// Centro dello sprite del parasite.
					SpriteRenderer parasiteSprite = parasite.GetComponent<SpriteRenderer>();
					Vector2 parasitePosition;

					if (parasiteSprite != null) {
						parasitePosition = parasiteSprite.bounds.center;
					} else {
						parasitePosition = parasite.transform.position;
					}

					float distance = Vector2.Distance(myPosition, parasitePosition);
					Vector2 directionToParasite = (parasitePosition - myPosition).normalized;
					RaycastHit2D hit = Physics2D.Raycast(myPosition, directionToParasite, distance, obstacleLayerMask);


					if (hit.collider != null || distance > targetDistance + 0.3f) {
						movement.SetDirection(directionToParasite);
						UpdateAnimation(directionToParasite);
					} else {
						movement.SetDirection(Vector2.zero);
						UpdateAnimation(Vector2.zero);
						currentState = State.shooting;
					}
				}
				break;
			}
			// =========================================================
			// SHOOTING
			// =========================================================
			case State.shooting: {
				movement.SetDirection(Vector2.zero);
				// Centro dello sprite della guardia.
				Vector2 origin = spriteRenderer.bounds.center;
				// Posizione del bersaglio.
				Vector2 targetPosition = GetTargetPosition();
				Vector2 directionToTarget = (targetPosition - origin).normalized;
				
				if (directionToTarget != Vector2.zero) {
					lastHorizontal = directionToTarget.x; 
					lastVertical = directionToTarget.y;
					animator.SetFloat("LastHorizontal", lastHorizontal);
					animator.SetFloat("LastVertical", lastVertical);
				}

				timer -= Time.deltaTime;
				float distance = Vector2.Distance(origin, targetPosition);

				RaycastHit2D hit = Physics2D.Raycast(origin, directionToTarget, distance, obstacleLayerMask);

				if (hit.collider != null || distance > targetDistance + 0.3f) {
					currentState = State.positioning;
					timer = 1.0f;
				} else if (timer <= 0) {
					animator.SetTrigger("Shooting");
					Shoot(false);
					timer = 1.0f;
				}

				if (parasite.currentState == Parasite.State.free) {
					currentState = State.waiting;
				}
				
				break;
			}
		}
	}
    // =========================================================
    // ANIMAZIONI
    // =========================================================
	private void UpdateAnimation(Vector2 direction) {
		if (animator == null) return;
	// Memorizzo l'ultima direzione SOLO quando la guardia
	// si sta effettivamente muovendo
		if (direction != Vector2.zero) {
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
	protected override void Die() {
		if (isDying) return;
		isDying = true;
		if (movement != null) {
			movement.speed = 0f;
			movement.SetDirection(Vector2.zero);
		}
		GameManager.gameManager.BossDefeated();
		Destroy(gameObject);
	}
	
	private void OnCollisionEnter2D(Collision2D collision) {
	// Il proiettile fa danno alla guardia boss
		if (collision.gameObject.CompareTag("Bullet") ){
			ReceiveDamage(10);
			return;
		}
	}
}