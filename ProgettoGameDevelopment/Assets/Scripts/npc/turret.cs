using UnityEngine;

public class turret : Nemico
{
	public float targetDistance = 3.0f;
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
		StatoAttuale = Stato.waiting;
	}

	void Start() {
		if (parassita == null) {
			parassita = FindFirstObjectByType<Parassita>();
		}

		if (parassita != null && parassita.StatoAttuale == Parassita.Stato.possessing) {
			StatoAttuale = Stato.shooting;
		} else {
			StatoAttuale = Stato.waiting;
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

		switch (StatoAttuale) {
		
		case Stato.waiting: {
			if (parassita.StatoAttuale ==Parassita.Stato.possessing) {
				StatoAttuale = Stato.shooting;
			}
			break;
		}
		
		case Stato.shooting: {
			// Centro dello sprite della guardia.
			Vector2 origine = spriteRenderer.bounds.center;
			// Posizione del bersaglio.
			Vector2 targetPosition = GetTargetPosition();
			Vector2 directionToTarget = (targetPosition - origine).normalized;
				
			if (directionToTarget != Vector2.zero) {
				lastHorizontal = directionToTarget.x; 
				lastVertical = directionToTarget.y;
				animator.SetFloat("LastHorizontal", lastHorizontal);
				animator.SetFloat("LastVertical", lastVertical);
			}

			timer -= Time.deltaTime;
			float distance = Vector2.Distance(origine, targetPosition);
			RaycastHit2D hit = Physics2D.Raycast(origine, directionToTarget, 
										distance, obstacleLayerMask);

			if (timer <= 0) {
				animator.SetTrigger("Shooting");
				Shoot(false);
				timer = 1.0f;
			}

			if (parassita.StatoAttuale == Parassita.Stato.libero) {
				StatoAttuale = Stato.waiting;
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

		if (direction != Vector2.zero) {
			lastHorizontal = direction.x;
			lastVertical = direction.y;
		}
        // Direzione attuale.
		//animator.SetFloat("Horizontal", direction.x);
		//animator.SetFloat("Vertical", direction.y);
		//animator.SetFloat("Speed", direction.magnitude); non serve per le torrette giusto?
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
		Destroy(gameObject, 1.5f);
	}
}