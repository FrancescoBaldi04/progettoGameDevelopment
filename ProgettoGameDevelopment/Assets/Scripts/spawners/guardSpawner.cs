using UnityEngine;
using UnityEngine.SceneManagement;
public class GuardSpawner : MonoBehaviour
{
	[SerializeField] private GameObject Guardia;
	[SerializeField] private Vector2 detectionBoxSize = new Vector2(30f, 30f);
	private Parasite Parasite;
	private int maxAmount;
	private float timer = 10.0f;
	private bool block = false;
	
	void Start() {
		int LivelloAttuale = SceneManager.GetActiveScene().buildIndex;
		if (LivelloAttuale == 1) {
			maxAmount = 3;
		} else if (LivelloAttuale == 2) {
			maxAmount = 4;
		} else {
			maxAmount = 0;
			block = true;
		}
	}

	void Update() {
		timer -= Time.deltaTime;
		if (timer <= 0) block = true;
		if (CheckForParasite() && maxAmount > 0) {
			int totalGuards = FindObjectsByType<Guard>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
			
			if (totalGuards == 0 && !block) {
				block = true;
				GameObject guard = Instantiate(Guardia, transform.position, transform.rotation);
			}
			
			if (totalGuards < maxAmount && timer <= 0) {
				block = false;
				timer = 10.0f;
				GameObject guard = Instantiate(Guardia, transform.position, transform.rotation);
			}
		}
	}
	
	protected bool CheckForParasite() {
		// CENTRO DELLO SPAWNER
		Vector2 position = this.transform.position;
		Collider2D[] objectsInside = Physics2D.OverlapBoxAll(position, detectionBoxSize, 0f);
		
		foreach (Collider2D collider in objectsInside) {
			
			if (collider.TryGetComponent<Parasite>(out _)) {
				return true;
			}
			
			if (collider.TryGetComponent<Guard>(out var g) && g.currentState == Guard.State.possessed) {
				return true;
			}
			
			if (collider.TryGetComponent<Scientist>(out var s) && s.currentState == Scientist.State.possessed) {
				return true;
			}
		}
		return false;
	}
}
