using UnityEngine;

public class turret : Nemico
{
	public float targetDistance = 3.0f;
	private float timer = 1.0f;
	private bool isDying = false;
	private Movement movement;
	private Animator animator;
	private int obstacleLayerMask;
	private float lastHorizontal = 0f;
	private float lastVertical = -1f;
	GameObject boss;
	protected override void Awake() {
		base.Awake();
		boss=GameObject.Find("GuardiaBoss");
		movement = GetComponent<Movement>();
		this.movement.speed=0f;
		animator = GetComponent<Animator>();
		obstacleLayerMask = LayerMask.GetMask("ground");
	}

	void Start() {
		if (parassita.StatoAttuale == Parassita.Stato.possessing)
		{
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
			movement.SetDirection(Vector2.zero);
			Vector2 origine;
		if (spriteRenderer != null) {
			origine = spriteRenderer.bounds.center;
		} else {
			origine = transform.position;
		}
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
		RaycastHit2D hit =Physics2D.Raycast(origine, directionToTarget,
									distance, obstacleLayerMask);
		if (timer <= 0) {
			animator.SetTrigger("Shooting");
			Shoot(false);
			timer = 1.0f;
		}
		if(parassita.StatoAttuale == Parassita.Stato.libero){
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


        // Memorizzo l'ultima direzione SOLO quando la guardia
        // si sta effettivamente muovendo.
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
		Destroy(gameObject, 1.5f);
	}
}