using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
	public Vector2 direction=Vector2.right;
	public Vector2 lastDirection=Vector2.right;
	public float speed=0.1f;
	private Rigidbody2D body;
	
	private void Start() {
		body=GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate() {
		this.body.linearVelocity = this.direction * (this.speed * 50);
	}
	
	public void SetDirection(Vector2 newdirection) {
		direction=newdirection;
		  if (newdirection != Vector2.zero) {
			lastDirection=newdirection;
		}
	}
}
