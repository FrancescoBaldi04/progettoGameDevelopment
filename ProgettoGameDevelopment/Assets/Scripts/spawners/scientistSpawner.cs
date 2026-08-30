using UnityEngine;
using UnityEngine.SceneManagement;
public class spawnerScientistBase : MonoBehaviour
{
	[SerializeField] private GameObject ScienziatoUomo;
	[SerializeField] private GameObject ScienziatoDonna;
	int quantitàMassima;
	bool genere;
	
	void Start() {
		int LivelloAttuale = SceneManager.GetActiveScene().buildIndex;
		if (LivelloAttuale == 0) {
			quantitàMassima = 3;
		} else if (LivelloAttuale == 1) {
			quantitàMassima = 2;
		} else {
			quantitàMassima = 1;
		}
	}

	void Update() {
		int numeroScienziati = FindObjectsByType<scientist>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
		if (numeroScienziati < quantitàMassima) {
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
