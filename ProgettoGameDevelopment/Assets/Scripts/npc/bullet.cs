using UnityEngine;

public class bullet : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D collision) {
		Destroy(gameObject, 1.5f);
	}
}
