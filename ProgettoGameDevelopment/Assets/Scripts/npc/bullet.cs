using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float lifeTime = 3f;
	void Start()
	{
		Destroy(gameObject, lifeTime); // Automatically destroyed after a set time
	}
	private void OnCollisionEnter2D(Collision2D collision) { // The outcome is placed within the collided objects
		Destroy(gameObject);
	}
}
