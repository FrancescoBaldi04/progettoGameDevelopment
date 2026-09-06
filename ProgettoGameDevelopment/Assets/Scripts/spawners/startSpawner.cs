using UnityEngine;

public class StartSpawner : MonoBehaviour
{
	[SerializeField] private GameObject femaleScientist;
	
	void Start() {
		GameObject scientist = Instantiate(femaleScientist, transform.position, transform.rotation);
		Destroy(gameObject);
	}

}
