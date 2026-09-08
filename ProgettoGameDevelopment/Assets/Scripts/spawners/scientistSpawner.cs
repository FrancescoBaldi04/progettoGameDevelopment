using UnityEngine;
using UnityEngine.SceneManagement;
public class ScientistSpawner : MonoBehaviour
{
	[SerializeField] private GameObject maleScientist;
	[SerializeField] private GameObject femaleScientist;
	[SerializeField] private Vector2 detectionBoxSize = new Vector2(30f, 30f);
	private int maxAmount;
	private float timer = 10.0f;
	private bool gender;
	private bool block = false;
	
	void Start() {
		// Set maximum active scientist threshold based on the current scene index
		int currentLevel = SceneManager.GetActiveScene().buildIndex;
		if (currentLevel == 0) 
		{
			maxAmount = 5;
		} 
		else 
		{
			maxAmount = 3;
		}
	}

	void Update() {
		timer -= Time.deltaTime;
		if (timer <= 0) block = true;
		if (CheckForParasite()) {
			int numberOfScientist = FindObjectsByType<Scientist>(FindObjectsInactive.Exclude, FindObjectsSortMode.None).Length;
			
			if (numberOfScientist == 0 && !block) {
				block = true;
				timer = 10.0f;
				if (gender) {
					gender = false;
					GameObject scientist = Instantiate(maleScientist, transform.position, transform.rotation);
				} else {
					gender = true;
					GameObject scientist = Instantiate(femaleScientist, transform.position, transform.rotation);
				}
			}
			
			if (numberOfScientist < maxAmount && timer <=0) {
				block = false;
				timer = 10.0f;
				if (gender) {
					gender = false;
					GameObject scientist = Instantiate(maleScientist, transform.position, transform.rotation);
				} else {
					gender = true;
					GameObject scientist = Instantiate(femaleScientist, transform.position, transform.rotation);
				}
			}
		}
	}
	
	protected bool CheckForParasite() // Checks if the Parasite, or a possessed body, is within the spawner's detection zone
	{ 
		// Center of the Spawner
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
