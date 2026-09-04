using UnityEngine;
using UnityEngine.SceneManagement;
public class guardSpawner : MonoBehaviour
{
	[SerializeField] private GameObject Guardia;
	[SerializeField] private Vector2 detectionBoxSize = new Vector2(20f, 20f);
	private Parassita parassita;
	private int quantitaMassima;
	private float timer = 10.0f;
	private bool block = false;
	
	void Start() {
		int LivelloAttuale = SceneManager.GetActiveScene().buildIndex;
		if (LivelloAttuale == 1) {
			quantitaMassima = 3;
		} else if (LivelloAttuale == 2) {
			quantitaMassima = 4;
		} else {
			quantitaMassima = 0;
			block = true;
		}
	}

	void Update() {
		timer -= Time.deltaTime;
		if (CheckForParassita() && quantitaMassima > 0) {
			int numeroGuardie = FindObjectsByType<guard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
			
			if (numeroGuardie == 0 && !block) {
				block = true;
				GameObject guard = Instantiate(Guardia, transform.position, transform.rotation);
			}
			
			if (numeroGuardie < quantitaMassima && timer <= 0) {
				block = false;
				timer = 10.0f;
				GameObject guard = Instantiate(Guardia, transform.position, transform.rotation);
			}
		}
	}
	
	protected bool CheckForParassita() {
		// CENTRO DELLO SPAWNER
		Vector2 position = this.transform.position;
		Collider2D[] objectsInside = Physics2D.OverlapBoxAll(position, detectionBoxSize, 0f);
		
		foreach (Collider2D collider in objectsInside) {
			
			if (collider.TryGetComponent<Parassita>(out _)) {
				return true;
			}
			
			if (collider.TryGetComponent<guard>(out var g) && g.StatoAttuale == guard.Stato.possessed) {
				return true;
			}
			
			if (collider.TryGetComponent<scientist>(out var s) && s.StatoAttuale == scientist.Stato.possessed) {
				return true;
			}
		}
		return false;
	}
}
