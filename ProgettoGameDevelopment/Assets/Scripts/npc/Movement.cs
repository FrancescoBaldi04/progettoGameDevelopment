using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
	public Vector2 direction=Vector2.right;
	public Vector2 lastDirection=Vector2.right;
	public float speed=5f;
	private Rigidbody2D body;
	
	private void Start() {
		body=GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate() {
		this.body.linearVelocity = direction * speed;
	}
	
	public void SetDirection(Vector2 newdirection) { // Sets the current movement direction
		direction=newdirection;
		  if (newdirection != Vector2.zero) { // Maintain last active facing direction when stopping
			lastDirection=newdirection;
		}
	}
}
