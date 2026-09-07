using UnityEngine;

public class Bullet : MonoBehaviour
{
	[SerializeField] private float lifeTime = 3f;
	void Start()
	{
		Destroy(gameObject, lifeTime); // AUTOMATICALLY DESTROYED AFTER A SET TIME
	}
	private void OnCollisionEnter2D(Collision2D collision) { // THE OUTCOME IS PLACED WITHIN THE COLLIDED OBJECTS
		Destroy(gameObject);
	}
}
