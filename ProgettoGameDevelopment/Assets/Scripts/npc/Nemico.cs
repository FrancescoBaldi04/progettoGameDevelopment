using UnityEngine;

public class Nemico : MonoBehaviour
{
	[SerializeField] protected int HitPoints = 60;
	public LayerMask Ground_Entities;
	public enum Stato {waiting, possessed, catching, escaping, positioning, shooting}; 
	public Stato StatoAttuale;
	public Parassita parassita;
	public bool up, down, right, left;
	public GameObject bulletPrefab;
	public float bulletSpeed=10f;
	public Transform firePoint;
	protected SpriteRenderer spriteRenderer;
	
	protected virtual void Awake() {
		parassita = FindFirstObjectByType<Parassita>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	public void Shoot(bool WhoIsShooting) {
		Vector3 uscitaProiettile = spriteRenderer.bounds.center;

		Vector2 direzione;

		if (WhoIsShooting)
		{
			// Il nemico è controllato dal Parassita
			Movement movement = GetComponent<Movement>();

			if (movement != null)
			{
				direzione = movement.lastDirection.normalized;
			}
			else
			{
				return;
			}
		}
		else
		{
			// Il nemico è controllato normalmente dall'IA
			Vector2 posizioneBersaglio = GetTargetPosition();

			direzione = (posizioneBersaglio - (Vector2)uscitaProiettile).normalized;
		}

			float angle =Mathf.Atan2(direzione.y, direzione.x) * Mathf.Rad2Deg;
			Quaternion rotazioneProiettile =Quaternion.Euler(0, 0, angle);
			GameObject bullet = Instantiate(bulletPrefab, uscitaProiettile, rotazioneProiettile);
			Collider2D bulletCollider = bullet.GetComponent<Collider2D>();

		if (bulletCollider == null)
		{
			Debug.LogError("Il prefab Proiettile non ha un Collider2D!");
			return;
		}

		if (WhoIsShooting)
		{
			Collider2D possessedCollider = parassita.GetComponent<Collider2D>();

			if (possessedCollider != null)
			{
				Physics2D.IgnoreCollision(bulletCollider, possessedCollider);
			}

			if (parassita.corpoPosseduto != null)
			{
				Collider2D corpoPossedutoCollider =
					parassita.corpoPosseduto.GetComponent<Collider2D>();

				if (corpoPossedutoCollider != null)
				{
					Physics2D.IgnoreCollision(bulletCollider, corpoPossedutoCollider);
				}
			}
		}
		else
		{
			Collider2D enemyCollider = GetComponent<Collider2D>();

			if (enemyCollider != null)
			{
				Physics2D.IgnoreCollision(bulletCollider, enemyCollider);
			}
		}
		
		Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
		
		if (rb != null) {
			rb.linearVelocity = direzione * bulletSpeed;
		}
	}
	public Vector2 GetBestDirection(Vector2 targetPosition, Vector2 exclude) {
		up = isFree(Vector2.up);
		down = isFree(Vector2.down);
		right = isFree(Vector2.right);
		left = isFree(Vector2.left);
		Vector2 currentPosition=new Vector2(this.transform.position.x, this.transform.position.y);
		Vector2 directionVector=targetPosition - currentPosition;
		float angleUp=Vector2.Angle(Vector2.up, directionVector);
		float angleDown=Vector2.Angle(Vector2.down, directionVector);
		float angleRight=Vector2.Angle(Vector2.right, directionVector);
		float angleLeft=Vector2.Angle(Vector2.left, directionVector);
		Vector2 bestDirection=Vector2.zero;
		float bestAngle=360.0f;
		if (up && angleUp<=bestAngle && exclude!=Vector2.up) {
			bestDirection=Vector2.up;
			bestAngle=angleUp;
		}
		if (down && angleDown<=bestAngle && exclude!=Vector2.down) {
			bestDirection=Vector2.down;
			bestAngle=angleDown;
		}
		if (right && angleRight<=bestAngle && exclude!=Vector2.right) {
			bestDirection=Vector2.right;
			bestAngle=angleRight;
		}
		if (left && angleLeft<=bestAngle && exclude!=Vector2.left) {
			bestDirection=Vector2.left;
			bestAngle=angleLeft;
		}
		return bestDirection;
	}
	public bool isFree(Vector2 direction){
		/*RaycastHit2D hitcast=Physics2D.BoxCast(this.transform.position, Vector2.one*0.75f, 0.0f, direction, 1.0f, this.Ground_Entities);
		return hitcast.collider==null;*/
		Vector2 boxSize = new Vector2(0.4f, 0.4f); 
		float checkDistance = 0.5f; 

		RaycastHit2D hitcast = Physics2D.BoxCast(this.transform.position, boxSize, 0.0f, direction, checkDistance, this.Ground_Entities);

		return hitcast.collider == null;
	}
	public void PrendiDanno(int danno) {
		HitPoints -= danno;
		Debug.Log(name + " ha ricevuto " + danno + " danni. HP: " + HitPoints);
		if (HitPoints <= 0){ 
			Die();
		}
	}

	protected virtual void Die() {
		Destroy(gameObject);
	}
	protected Vector2 GetTargetPosition() {
		if (parassita.StatoAttuale == Parassita.Stato.possessing && parassita.corpoPosseduto != null) {
			return parassita.GetCorpoPossedutoPosition();
		}
		return parassita.transform.position;
	}
	
	public Vector2 GetEscapeDirection(Vector2 dangerPosition)
	{
		Vector2 currentPos = transform.position;
		Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

		Vector2 bestDir = Vector2.zero;
		float maxDistance = -1f;

		foreach (Vector2 dir in directions)
		{
			if (isFree(dir))
			{
				Vector2 nextPos = currentPos + dir;
				float distanceToDanger = Vector2.Distance(nextPos, dangerPosition);

				if (distanceToDanger > maxDistance)
				{
					maxDistance = distanceToDanger;
					bestDir = dir;
				}
			}
		}

		return bestDir;
	}
}