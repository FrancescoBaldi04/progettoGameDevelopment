using UnityEngine;
using UnityEngine.SceneManagement;
public class spawnerScientistBase : MonoBehaviour
{
	[SerializeField] private GameObject ScienziatoUomo;
	[SerializeField] private GameObject ScienziatoDonna;
	[SerializeField] private Vector2 detectionBoxSize = new Vector2(20f, 20f);
	private int quantitaMassima;
	private float timer = 10.0f;
	private bool genere;
	private bool block = false;
	
	void Start() {
		int LivelloAttuale = SceneManager.GetActiveScene().buildIndex;
		if (LivelloAttuale == 0) {
			quantitaMassima = 5;
		} else if (LivelloAttuale == 4) {
			quantitaMassima = 1;
		} else {
			quantitaMassima = 3;
		}
	}

	void Update() {
		timer -= Time.deltaTime;
		if (CheckForParassita()) {
			int numeroScienziati = FindObjectsByType<scientist>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
			
			if (numeroScienziati == 0 && !block) {
				block = true;
				if (genere) {
					genere = false;
					GameObject scientist = Instantiate(ScienziatoUomo, transform.position, transform.rotation);
				} else {
					genere = true;
					GameObject scientist = Instantiate(ScienziatoDonna, transform.position, transform.rotation);
				}
			}
			
			if (numeroScienziati < quantitaMassima && timer <=0) {
				block = false;
				timer = 10.0f;
				if (genere) {
					genere = false;
					GameObject scientist = Instantiate(ScienziatoUomo, transform.position, transform.rotation);
				} else {
					genere = true;
					GameObject scientist = Instantiate(ScienziatoDonna, transform.position, transform.rotation);
				}
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
