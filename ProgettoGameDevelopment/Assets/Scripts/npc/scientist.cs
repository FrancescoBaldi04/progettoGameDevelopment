
using UnityEngine;

public class Scientist : Enemy
{
	private bool isDying = false;
	private float lastHorizontal = 0;
	private float lastVertical = -1f;
	private Movement movement;
	private Animator animator;
	private float timer = 30.0f;

	protected override void Awake() {
		base.Awake();
		movement = GetComponent<Movement>();
		animator = GetComponent<Animator>();
	}

	void Start() {
		currentState = State.idle;
	}

	void Update() {
		if (isDying) return;
		
		if (hitPoints <= 0) {
			Die();
			return;
		}
		
		switch (currentState) {
			
			// WAITING
			
			case State.idle: {
				timer -= Time.deltaTime;
				
				if (timer <= 0) {
					Die();
					return;
				}
				
				if (!CheckForParasite()) {
					Vector2 direction = RandomMovement();
					movement.SetDirection(direction);
					UpdateAnimation(direction);
				} else if (parasite.currentState == Parasite.State.possessing) {
					currentState = State.escaping;
				} else {
					currentState = State.catching;
				}
				break;
			}
			
			// CATCHING
			
			case State.catching: {
				if (!CheckForParasite()) {
					timer = 30.0f;
					currentState = State.idle;
					movement.SetDirection(Vector2.zero);
					UpdateAnimation(Vector2.zero);
				break;
				}
				
				// CENTER OF THE SCIENTIST'S SPRITE
				Vector2 scientistPosition = spriteRenderer.bounds.center;
				// CENTER OF THE PARASITE'S SPRITE
				Vector2 parasitePosition = parasite.GetComponent<SpriteRenderer>().bounds.center;
				float distance = Vector2.Distance(scientistPosition, parasitePosition);

				if (distance > 0.1f) {
					Vector2 captureDirection = GetBestDirection(parasitePosition, Vector2.zero);
					movement.SetDirection(captureDirection);
					UpdateAnimation(captureDirection);
				} else {
					movement.SetDirection(Vector2.zero);
					UpdateAnimation(Vector2.zero);
				}
				
				if (parasite.currentState == Parasite.State.possessing) {
					currentState = State.escaping;
				}
			break;
			}
			
			// ESCAPING
			
			case State.escaping: {
				 if (parasite.currentState == Parasite.State.free) {
					timer = 30.0f;
					currentState = State.idle;
					movement.SetDirection(Vector2.zero);
					UpdateAnimation(Vector2.zero);
				break;
				}
				Vector2 threatPosition = GetTargetPosition();
				Vector2 fleeDirection = GetEscapeDirection(threatPosition);
				movement.SetDirection(fleeDirection);
				UpdateAnimation(fleeDirection);
				
				if (parasite.currentState == Parasite.State.free) {
					currentState = State.catching;
				}
			break;
			}
			
			// POSSESSED
			
			case State.possessed: {
				if (parasite.currentState == Parasite.State.free) {
					this.hitPoints = 0;
					break;
				}
				// PLAYER INPUT
				Vector2 playerInput = InputManager.movement;
				movement.SetDirection(playerInput);
				UpdateAnimation(playerInput);
			break;
			}
		}
	}
	
	
	// DEATH
	
	protected override void Die() {
		if (isDying) return;
		isDying = true;
		
		if (movement != null) movement.speed = 0f;
		
		Destroy(gameObject);
	}
	
	// ANIMATIONS
	
	private void UpdateAnimation(Vector2 direction) {
		if (animator == null) return;

		if (direction != Vector2.zero) {
			lastHorizontal = direction.x;
			lastVertical = direction.y;
		}

		animator.SetFloat("Horizontal",direction.x);
		animator.SetFloat("Vertical",direction.y);
		animator.SetFloat("Speed",direction.sqrMagnitude);
		animator.SetFloat("LastHorizontal",lastHorizontal);
		animator.SetFloat("LastVertical",lastVertical);
	}
	
	// COLLISIONS
	
	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Bullet")) {
			if (parasite.possessedBody == gameObject) {
				parasite.TakeDamage(10);
			} else {
				ReceiveDamage(10);
			}
		return;
		}

		Parasite collidedParasite = collision.gameObject.GetComponent<Parasite>();

		if (currentState == State.catching && collidedParasite != null) {
			PlayerJump playerJump = collidedParasite.GetComponent<PlayerJump>();
			if (playerJump != null && !playerJump.isInAir) {
				collidedParasite.Die();
			}
		}
	}
}

