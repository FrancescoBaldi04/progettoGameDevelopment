using UnityEngine;

public class TrojanHorsePickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //  Parassita libero
        Parassita parassita = other.GetComponent<Parassita>();

        if (parassita != null)
        {
            GameManager.gameManager.UnlockTrojanHorse();
            Destroy(gameObject);
            return;
        }

        //  il collider appartiene a un NPC
        Nemico nemico = other.GetComponent<Nemico>();

        if (nemico != null && nemico.parassita != null)
        {
            parassita = nemico.parassita;

            
            if (parassita.corpoPosseduto == nemico.gameObject)
            {
                GameManager.gameManager.UnlockTrojanHorse();
                Destroy(gameObject);
            }
        }
    }
}