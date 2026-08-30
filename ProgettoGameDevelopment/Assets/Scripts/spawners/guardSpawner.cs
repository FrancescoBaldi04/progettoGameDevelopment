using UnityEngine;

public class guardSpawner : MonoBehaviour
{
	[SerializeField] private GameObject Guardia;

	void Start() {
		
	}

	void Update() {
		int numeroGuardie = FindObjectsByType<guard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
		if (numeroGuardie<2) {
			GameObject guard = Instantiate(Guardia, transform.position, transform.rotation);
		}
	}
}
