using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
	public Vector2 direction=Vector2.right;
	public float speed=5.0f;
	private Rigidbody2D body;
	private void Start() {
		body=GetComponent<Rigidbody2D>();
	}
	private void move() {
		Vector2 startPosition=this.body.position;
		Vector2 translation=this.direction*this.speed;
		this.body.MovePosition(startPosition+translation);
	}
	public void SetDirection(Vector2 newdirection) {
		direction=newdirection;
	}
}
