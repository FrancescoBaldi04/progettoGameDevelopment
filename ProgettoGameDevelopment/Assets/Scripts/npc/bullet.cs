using UnityEngine;

public class bullet : MonoBehaviour
{
	[SerializeField] private float lifeTime = 3f;

    void Start()
    {
        Destroy(gameObject, lifeTime); // se non colpisce niente entro il suo lifetime lo distruggiamo automaticamente
    }
    private void OnCollisionEnter2D(Collision2D collision) {
		Destroy(gameObject);
	}
}
