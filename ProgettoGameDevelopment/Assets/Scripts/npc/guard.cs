
using UnityEngine;

public class Guard : Enemy
{
	public float targetDistance = 7.5f;
	private float timer = 30.0f;
	private bool isDying = false;
	private Movement movement;
	private Animator animator;
	private int obstacleLayerMask;
	private float lastHorizontal = 0f;
	private float lastVertical = -1f;
	
	protected override void Awake() {
		base.Awake();

		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();

		obstacleLayerMask = LayerMask.GetMask("ground");
	}


	void Start() {
		// All Guards start idle, CheckForParasite() will activate them
		currentState = State.idle;
		UpdateAnimation(Vector2.zero);
	}


	void Update() {
		if (isDying) return;
		
		if (hitPoints <= 0)
		{
			Die();
			return;
		}

		// State Machine
		switch (currentState) {
			
			// Waiting
			
			case State.idle: {
				timer-=Time.deltaTime;
				
				if (timer <= 0) {
					Die();
					return;
				}
					
				if (!CheckForParasite()) {
					Vector2 direction = RandomMovement();
					movement.SetDirection(direction);
					UpdateAnimation(direction);
				} else if (parasite.currentState == Parasite.State.possessing) {
					currentState = State.positioning;
					timer = 1.0f;
				} else {
					currentState = State.escaping;
					timer = 1.0f;
				}
				break;
			}
			
			// Escaping
			
			case State.escaping: { 
				if (!CheckForParasite()){
					timer = 30.0f;
					currentState = State.idle;
					movement.SetDirection(Vector2.zero);
					UpdateAnimation(Vector2.zero);
				break;
				}

				Vector2 guardPosition = spriteRenderer.bounds.center;
				Vector2 parasitePosition = parasite.GetComponent<SpriteRenderer>().bounds.center;
				Vector2 versoDiFuga = -GetBestDirection(parasitePosition, Vector2.zero);
				movement.SetDirection(versoDiFuga);
				UpdateAnimation(versoDiFuga);

				if (parasite.currentState == Parasite.State.possessing) {
					currentState = State.positioning;
				}
				break;
			}
			
			// Positioning
			
			case State.positioning: { 
				if (parasite.currentState == Parasite.State.free) {
					currentState = State.escaping;
					break;
				}
				if (!CheckForParasite()) {
					timer = 30.0f;
					currentState = State.idle;
					movement.SetDirection(Vector2.zero);
					UpdateAnimation(Vector2.zero);
				break;
				}
				
				if (movement != null && parasite != null) {
					Vector2 myPosition = spriteRenderer.bounds.center;
					SpriteRenderer parasiteSprite = parasite.GetComponent<SpriteRenderer>();
					Vector2 parasitePosition;

					if (parasiteSprite != null) {
						parasitePosition = parasiteSprite.bounds.center;
					} else {
						parasitePosition = parasite.transform.position;
					}

					float distance = Vector2.Distance(myPosition, parasitePosition);
					Vector2 directionToParasite = (parasitePosition - myPosition).normalized;
					
					// Check for physical obstacles blocking line of sight
					RaycastHit2D hit = Physics2D.Raycast(myPosition, directionToParasite, distance, obstacleLayerMask);

					if (hit.collider != null || distance > targetDistance + 0.3f) {
						movement.SetDirection(directionToParasite);
						UpdateAnimation(directionToParasite);
					} else {
						// Stop moving once clear line of sight is secured
						movement.SetDirection(Vector2.zero);
						UpdateAnimation(Vector2.zero);
						currentState = State.shooting;
					}
				}
				break;
			}
			
			// Shooting

			case State.shooting: { // Remains stationary and fires at target on interval
				movement.SetDirection(Vector2.zero);
				Vector2 origin = spriteRenderer.bounds.center;
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
					currentState = State.escaping;
				}
				
				break;
			}
			
			// Possessed
	
			case State.possessed: {
				if (parasite.currentState == Parasite.State.free) {
					hitPoints = 0;
					break;
				}
				// Player input
				Vector2 inputGiocatore = InputManager.movement;
				movement.SetDirection(inputGiocatore);
				UpdateAnimation(inputGiocatore);
				break;
			}
		}
	}
	
	// Animations
	
	private void UpdateAnimation(Vector2 direction) {
		if (animator == null) return;
		// MEMORIZE LAST POSITION ONLY IF GUARD MOVES
		if (direction != Vector2.zero) {
			lastHorizontal = direction.x;
			lastVertical = direction.y;
		}

		animator.SetFloat("Horizontal", direction.x);
		animator.SetFloat("Vertical", direction.y);
		animator.SetFloat("Speed", direction.magnitude);
		animator.SetFloat("LastHorizontal", lastHorizontal);
		animator.SetFloat("LastVertical", lastVertical);
	}
	
	// Death

	protected override void Die() {
		if (isDying) return;
		isDying = true;
		
		if (movement != null) {
			movement.speed = 0f;
			movement.SetDirection(Vector2.zero);
		}
		Destroy(gameObject);
	}
	
	// Handles projectile damage
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Bullet")) {
			if (parasite.possessedBody == gameObject) {
				parasite.TakeDamage(10);
			} else {
				ReceiveDamage(10);
			}
		return;
		}
	}
}
