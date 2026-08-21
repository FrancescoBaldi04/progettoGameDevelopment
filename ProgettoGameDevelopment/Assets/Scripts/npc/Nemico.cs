using UnityEngine;

public class Nemico : MonoBehaviour
{
	[SerializeField] protected int HitPoints = 60;
	public LayerMask humanLayer;
	public enum Stato {possessed, catching, escaping, positioning, shooting, waiting}; 
	public Stato StatoAttuale;
	public Parassita parassita;
	public bool up, down, right, left;
	public GameObject bulletPrefab;
	public float bulletSpeed=10f;
	public Transform firePoint;
	public void Shoot() {
		Vector3 uscitaProiettile= (firePoint != null) ? firePoint.position : transform.position;
		
		Vector2 direzione = (parassita.transform.position - transform.position).normalized;
		
		float angle = Mathf.Atan2(direzione.y, direzione.x) * Mathf.Rad2Deg; // ruoto il proiettile in modo che guardi sempre verso il parassita
    	Quaternion rotazioneProiettile = Quaternion.Euler(0, 0, angle);

		GameObject bullet=Instantiate(bulletPrefab, uscitaProiettile, rotazioneProiettile);
		Rigidbody2D rb=bullet.GetComponent<Rigidbody2D>();
		if (rb!=null) {
			rb.linearVelocity = direzione * bulletSpeed;
		}
	}
	public Vector2 GetBestDirection(Vector2 targetPosition, Vector2 exclude)
	{
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
	protected virtual void Awake()
	{
    	parassita = FindFirstObjectByType<Parassita>();
	}
	public bool isFree(Vector2 direction){
		RaycastHit2D hitcast=Physics2D.BoxCast(
			this.transform.position,
			Vector2.one*0.75f,
			0.0f,
			direction,
			1.0f,
			this.humanLayer
		);
		return hitcast.collider==null;
	}
	public void PrendiDanno(int danno)
	{
		HitPoints -= danno;

		Debug.Log(name + " ha ricevuto " + danno + " danni. HP: " + HitPoints);

		if (HitPoints <= 0){ 
            Die();
		}
	}

	protected virtual void Die() // comportamento base distrugge l'oggetto però i figli possono sovrascrivere il metodo
	{
		Destroy(gameObject);
	}
	
}