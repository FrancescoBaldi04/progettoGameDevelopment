using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Movement : MonoBehaviour
{
	public Vector2 direction=Vector2.right;
	public float speed=0.1f;
	private Rigidbody2D body;
	
	private void Start() {
		body=GetComponent<Rigidbody2D>();
	}
	
	private void FixedUpdate() {
		Vector2 translation=this.direction*this.speed;
		//this.body.MovePosition(this.body.position+translation);
		this.body.linearVelocity = this.direction * (this.speed * 50f);
	}
	
	public void SetDirection(Vector2 newdirection) {
		
			direction=newdirection;
		
	}
}
