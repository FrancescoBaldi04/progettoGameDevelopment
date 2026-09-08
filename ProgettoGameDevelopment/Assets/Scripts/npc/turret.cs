using UnityEngine;

public class Turret : Enemy
{
	public float targetDistance = 7.5f;
	private float timer = 1.0f;
	private bool isDying = false;
	private Animator animator;
	private int obstacleLayerMask;
	private float lastHorizontal = 0f;
	private float lastVertical = -1f;
	GameObject boss;
	protected override void Awake() {
		base.Awake();
		boss=GameObject.Find("GuardiaBoss");
		animator = GetComponent<Animator>();
		obstacleLayerMask = LayerMask.GetMask("ground");
		currentState = State.waiting;
	}

	void Start() {
		if (parasite == null) {
			parasite = FindFirstObjectByType<Parasite>();
		}

		// Activate immediately if Parasite is already possessing an NPC
		if (parasite != null && parasite.currentState == Parasite.State.possessing) {
			currentState = State.shooting;
		} else {
			currentState = State.waiting;
		}
	
		UpdateAnimation(Vector2.zero);
	}

	void Update() {
		if (isDying) return;
		if (boss == null) 
		{
			Die();
			return;
		}

		// State Machine
		switch (currentState) {
		
			case State.waiting: {
				if (parasite.currentState ==Parasite.State.possessing) {
					currentState = State.shooting;
				}
				break;
			}
			
			case State.shooting: {
				// Center of the Turret's sprite
				Vector2 origine = spriteRenderer.bounds.center;
				// Target's position
				Vector2 targetPosition = GetTargetPosition();
				Vector2 directionToTarget = (targetPosition - origine).normalized;
					
				if (directionToTarget != Vector2.zero) {
					lastHorizontal = directionToTarget.x; 
					lastVertical = directionToTarget.y;
					animator.SetFloat("LastHorizontal", lastHorizontal);
					animator.SetFloat("LastVertical", lastVertical);
				}

				timer -= Time.deltaTime;
				
				// Check line of sight againts obstacles
				float distance = Vector2.Distance(origine, targetPosition);
				RaycastHit2D hit = Physics2D.Raycast(origine, directionToTarget, distance, obstacleLayerMask);

				if (timer <= 0) {
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
	
	// Death
	
	protected override void Die() {
		if (isDying) return;
		isDying = true;
		Destroy(gameObject, 0.5f);
	}
	
	// Animations
	
	private void UpdateAnimation(Vector2 direction) {
		if (animator == null) return;

		if (direction != Vector2.zero) {
			lastHorizontal = direction.x;
			lastVertical = direction.y;
		}
		animator.SetFloat("LastHorizontal", lastHorizontal);
		animator.SetFloat("LastVertical", lastVertical);
	}
}