using UnityEngine;
using UnityEngine.SceneManagement;
public class spawnerScientistBase : MonoBehaviour
{
	[SerializeField] private GameObject ScienziatoUomo;
	[SerializeField] private GameObject ScienziatoDonna;
	int quantitaMassima;
	bool genere;
	
	void Start() {
		int LivelloAttuale = SceneManager.GetActiveScene().buildIndex;
		if (LivelloAttuale == 0) {
			quantitaMassima = 3;
		} else if (LivelloAttuale == 1) {
			quantitaMassima = 2;
		} else {
			quantitaMassima = 1;
		}
	}

	void Update() {
		int numeroScienziati = FindObjectsByType<scientist>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
		if (numeroScienziati < quantitaMassima) {
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
