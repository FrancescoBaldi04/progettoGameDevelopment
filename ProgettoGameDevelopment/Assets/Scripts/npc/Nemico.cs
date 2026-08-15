using UnityEngine;

public class Nemico : MonoBehaviour
{
	[SerializeField] protected int HitPoints = 60;
	public LayerMask humanLayer;
	public bool up, down, right, left;
	public Vector2 GetBestDirection(Vector2 targetPosition)
	{
		Vector2 currentPosition=new Vector2(this.transform.position.x, this.transform.position.y);
		Vector2 directionVector=targetPosition - currentPosition;
		float angleUp=Vector2.Angle(Vector2.up, directionVector);
		float angleDown=Vector2.Angle(Vector2.down, directionVector);
		float angleRight=Vector2.Angle(Vector2.right, directionVector);
		float angleLeft=Vector2.Angle(Vector2.left, directionVector);
		Vector2 bestDirection=Vector2.zero;
		float bestAngle=359.0f;
		
		if (up && angleUp<=bestAngle) {
			bestDirection=Vector2.up;
			bestAngle=angleUp;
		}
		if (down && angleDown<=bestAngle) {
			bestDirection=Vector2.down;
			bestAngle=angleDown;
		}
		if (right && angleRight<=bestAngle) {
			bestDirection=Vector2.right;
			bestAngle=angleRight;
		}
		if (left && angleLeft<=bestAngle) {
			bestDirection=Vector2.left;
			bestAngle=angleLeft;
		}
		return bestDirection;
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
}