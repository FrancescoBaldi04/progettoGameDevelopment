using UnityEngine;

public class startSpawner : MonoBehaviour
{
	[SerializeField] private GameObject ScienziatoDonna;
	
	void Start() {
		GameObject scientist = Instantiate(ScienziatoDonna, transform.position, transform.rotation);
		Destroy(gameObject);
	}

}
