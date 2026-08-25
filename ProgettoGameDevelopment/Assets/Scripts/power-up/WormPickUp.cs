using UnityEngine;

public class WormPickUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Parassita parassita = other.GetComponent<Parassita>();

        if (parassita != null)
        {
            GameManager.gameManager.UnlockWorm();

            Destroy(gameObject);
        }
        Nemico nemico = other.GetComponent<Nemico>();

        if (nemico != null && nemico.parassita != null)
        {
            parassita = nemico.parassita;

            GameManager.gameManager.UnlockWorm();

            Destroy(gameObject);
        }
    }
}
