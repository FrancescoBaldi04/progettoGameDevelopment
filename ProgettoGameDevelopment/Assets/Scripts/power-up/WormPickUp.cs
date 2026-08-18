using UnityEngine;

public class WormPickUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Parassita parassita = other.GetComponent<Parassita>();

        if (parassita != null)
        {
            GameManager.gameManager.UnlockWorm();

            Destroy(gameObject);
        }
    }
}
