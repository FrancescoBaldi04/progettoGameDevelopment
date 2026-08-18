using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextScene;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Parassita>() != null)
        {
            SceneManager.LoadScene(nextScene);
        }
    }
}