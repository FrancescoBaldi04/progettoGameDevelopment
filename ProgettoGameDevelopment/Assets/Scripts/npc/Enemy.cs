using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
	[SerializeField] protected int hitPoints = 60;
	public LayerMask groundEntities;
	public enum State {idle, waiting, possessed, catching, escaping, positioning, shooting};
	public State currentState;
	public Parasite parasite;
	public bool up, down, right, left;
	public GameObject bulletPrefab;
	public float bulletSpeed = 10f;
	protected SpriteRenderer spriteRenderer;
	[SerializeField] private Vector2 detectionBoxSize = new Vector2(15f, 15f);
	protected Vector2 randomDirection = Vector2.zero;
	[SerializeField] protected float randomCheckDistance = 1.5f;
	[SerializeField] protected float randomCheckSize = 0.75f;

	protected virtual void Awake() {
		parasite = FindFirstObjectByType<Parasite>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	// =========================================================
	// SPRITE POSITION
	// =========================================================
	protected Vector2 GetSpritePosition() {
		return spriteRenderer.bounds.center;
	}
	// =========================================================
	// SHOOT
	// =========================================================
	public void Shoot(bool WhoIsShooting) {
		Vector3 firePoint = spriteRenderer.bounds.center;
		Vector2 direction;
		
		if (WhoIsShooting) {
			Movement movement = GetComponent<Movement>();

			if (movement != null) {
				direction = movement.lastDirection.normalized;
			} else {
				return;
			}
		} else {
			Vector2 targetPosition = GetTargetPosition();
			direction = (targetPosition - (Vector2)firePoint).normalized;
		}

		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion bulletRotation = Quaternion.Euler(0, 0, angle);
		GameObject bullet = Instantiate(bulletPrefab,firePoint,bulletRotation);
		Collider2D bulletCollider = bullet.GetComponent<Collider2D>();

		if (bulletCollider == null) {
			return;
		}

		if (WhoIsShooting) {
			Collider2D possessedCollider = parasite.GetComponent<Collider2D>();

			if (possessedCollider != null) {
				Physics2D.IgnoreCollision(bulletCollider,possessedCollider);
			}

			if (parasite.possessedBody != null) {
				Collider2D possessedBodyCollider = parasite.possessedBody.GetComponent<Collider2D>();

				if (possessedBodyCollider != null) {
					Physics2D.IgnoreCollision(bulletCollider,possessedBodyCollider);
				}
			}
		} else {
			Collider2D enemyCollider = GetComponent<Collider2D>();

			if (enemyCollider != null) {
				Physics2D.IgnoreCollision(bulletCollider,enemyCollider);
			}
		}

		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		
		if (rb != null) {
			rb.linearVelocity = direction * bulletSpeed;
		}
	}
	
	// =========================================================
	// DIRECTION TOWARDS TARGET
	// =========================================================
	public Vector2 GetBestDirection(Vector2 targetPosition,Vector2 exclude) {
		up = IsFree(Vector2.up);
		down = IsFree(Vector2.down);
		right = IsFree(Vector2.right);
		left = IsFree(Vector2.left);
		
		// CENTER OF THE SPRITE
		Vector2 currentPosition = GetSpritePosition();
		Vector2 directionVector = targetPosition - currentPosition;
		
		float angleUp = Vector2.Angle(Vector2.up,directionVector);
		float angleDown = Vector2.Angle(Vector2.down,directionVector);
		float angleRight = Vector2.Angle(Vector2.right,directionVector);
		float angleLeft = Vector2.Angle(Vector2.left,directionVector);
		
		Vector2 bestDirection = Vector2.zero;
		float bestAngle = 360f;

		if (up && angleUp <= bestAngle && exclude != Vector2.up) {
			bestDirection = Vector2.up;
			bestAngle = angleUp;
		}

		if (down && angleDown <= bestAngle && exclude != Vector2.down) {
			bestDirection = Vector2.down;
			bestAngle = angleDown;
		}

		if (right && angleRight <= bestAngle && exclude != Vector2.right) {
			bestDirection = Vector2.right;
			bestAngle = angleRight;
		}

		if (left && angleLeft <= bestAngle && exclude != Vector2.left) {
			bestDirection = Vector2.left;
			bestAngle = angleLeft;
		}

		return bestDirection;
	}
	// =========================================================
	// CONTROL FREE DIRECTION
	// =========================================================
	public bool IsFree(Vector2 direction) {
		Vector2 spritePosition = spriteRenderer.bounds.center;
		Vector2 checkPosition = spritePosition + direction * randomCheckDistance;
		Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPosition,new Vector2(randomCheckSize, randomCheckSize),0f);

		foreach (Collider2D collider in colliders) {
			if (collider.CompareTag("Wall") || collider.CompareTag("Npc")) {
				return false;
			}
		}
		return true;
	}
	// =========================================================
	// DAMAGE
	// =========================================================
	public void ReceiveDamage(int damage) {
		hitPoints -= damage;
		
		if (hitPoints <= 0) {
			Die();
		}
	}

	protected virtual void Die() {
		Destroy(gameObject);
	}
	// =========================================================
	// POSITION OF TARGET
	// =========================================================
	protected Vector2 GetTargetPosition() {
		if (parasite.currentState == Parasite.State.possessing && parasite.possessedBody != null) {
			return parasite.GetPossessedBodyPosition();
		}
		return parasite.transform.position;
	}
	// =========================================================
	// ESCAPE DIRECTION
	// =========================================================
	public Vector2 GetEscapeDirection(Vector2 dangerPosition) {
		// CENTER OF THE SPRITE
		Vector2 currentPosition = GetSpritePosition();
		Vector2[] directions = {Vector2.up,Vector2.down,Vector2.left,Vector2.right};
		Vector2 bestDirection = Vector2.zero;
		float maxDistance = -1f;
		
		foreach (Vector2 dir in directions) {
			if (IsFree(dir)) {
				Vector2 nextPos = currentPosition + dir;
				float distanceToDanger = Vector2.Distance(nextPos,dangerPosition);

				if (distanceToDanger > maxDistance) {
					maxDistance = distanceToDanger;
					bestDirection = dir;
				}
			}
		}
		return bestDirection;
	}
	// =========================================================
	// CHECK FOR PARASITE
	// =========================================================
	protected bool CheckForParasite() {
		// CENTER OF THE SPRITE
		Vector2 spritePosition = GetSpritePosition();
		Collider2D[] objectsInside = Physics2D.OverlapBoxAll(spritePosition,detectionBoxSize,0f);

		foreach (Collider2D collider in objectsInside) {
			
			if (collider.TryGetComponent<Parasite>(out _)) {
				return true;
			}
			
			if (collider.TryGetComponent<Guard>(out var g) && g.currentState == Guard.State.possessed) {
				return true;
			}
			
			if (collider.TryGetComponent<Scientist>(out var s) && s.currentState == Scientist.State.possessed) {
				return true;
			}
		}
		return false;
	}
	// =========================================================
	// CHOOSE RANDOM DIRECTION
	// =========================================================
	protected Vector2 GetRandomDirection() {
		Vector2[] directions =
		{Vector2.up,Vector2.down,Vector2.left,Vector2.right};
		List<Vector2> availableDirections =new List<Vector2>();
		
		foreach (Vector2 direction in directions) {
			if (!WallInDirection(direction)) {
				availableDirections.Add(direction);
			}
		}

		if (availableDirections.Count == 0) {
			return Vector2.zero;
		}

		return availableDirections[Random.Range(0, availableDirections.Count)];
	}
	// =========================================================
	// CHECK FOR WALLS
	// =========================================================
	protected bool WallInDirection(Vector2 direction) {
		// CENTER OF THE SPRITE
		Vector2 spritePosition = GetSpritePosition();
		Vector2 checkPosition = spritePosition + direction * randomCheckDistance;
		Collider2D[] colliders = Physics2D.OverlapBoxAll(checkPosition,new Vector2(randomCheckSize,randomCheckSize),0f);
		
		foreach (Collider2D collider in colliders) {
			if (collider.CompareTag("Wall") || collider.CompareTag("Npc")) {
				return true;
			}
		}
		
		return false;
	}
	// =========================================================
	//  RANDOM MOVEMENT
	// =========================================================
	protected Vector2 RandomMovement() {
		if (randomDirection == Vector2.zero || WallInDirection(randomDirection)) {
			randomDirection = GetRandomDirection();
		}
		
		return randomDirection;
	}
	
}

