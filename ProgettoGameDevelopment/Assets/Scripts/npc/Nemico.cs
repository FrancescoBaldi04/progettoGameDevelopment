using UnityEngine;

public class Nemico : MonoBehaviour
{
	[SerializeField] protected int HitPoints = 60;
	public LayerMask humanLayer;
	public enum Stato {possessed, catching, escaping, positioning, shooting, waiting};
	public Stato StatoAttuale;
	public Parassita parassita;
	public bool up, down, right, left;
	public Vector2 GetBestDirection(Vector2 targetPosition, Vector2 exclude)
	{
		Vector2 currentPosition=new Vector2(this.transform.position.x, this.transform.position.y);
		Vector2 directionVector=targetPosition - currentPosition;
		float angleUp=Vector2.Angle(Vector2.up, directionVector);
		float angleDown=Vector2.Angle(Vector2.down, directionVector);
		float angleRight=Vector2.Angle(Vector2.right, directionVector);
		float angleLeft=Vector2.Angle(Vector2.left, directionVector);
		Vector2 bestDirection=Vector2.zero;
		float bestAngle=359.0f;
		
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
            // qui eventualmente gestisci la morte
		Destroy(gameObject);
		}
	}
	public void OnTriggerEnter2D(Collider2D collider) {
		if (this.StatoAttuale==Stato.catching) {
			if (parassita!=null) {
				Vector2 target=new Vector2(parassita.transform.position.x, parassita.transform.position.y)
							+
							(0*parassita.movement);
				Movement movement=GetComponent<Movement>();
				Vector2 avoiddirection=Vector2.zero;
				if(movement!=null) {
					avoiddirection=-1*movement.direction;
				}
				Vector2 parassitaPosition=new Vector2 (parassita.transform.position.x, parassita.transform.position.y);
				Vector2 bestdirection =GetBestDirection(parassitaPosition, avoiddirection);
				movement.SetDirection(bestdirection);
			}
		}
	}
}