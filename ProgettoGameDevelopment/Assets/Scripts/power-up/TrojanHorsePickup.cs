using UnityEngine;

public class TrojanHorsePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Parassita parassita = other.GetComponent<Parassita>();

        // Caso 1: il parassita è libero
        if (parassita != null)
        {
            GameManager.gameManager.UnlockTrojanHorse();
            Destroy(gameObject);
            return;
        }

        // Caso 2: il parassita è dentro un NPC
        Nemico nemico = other.GetComponent<Nemico>();

        if (nemico != null && nemico.parassita != null)
        {
            parassita = nemico.parassita;

            GameManager.gameManager.UnlockTrojanHorse();
            Destroy(gameObject);
        }
    }
}