using UnityEngine;

public class TrojanHorsePickup : MonoBehaviour
{
    private PickupMessage pickupMessage;

    private void Start()
    {
       pickupMessage = FindFirstObjectByType<PickupMessage>(
        FindObjectsInactive.Include
    );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Parassita libero
        Parassita parassita = other.GetComponent<Parassita>();

        if (parassita != null)
        {
            GameManager.gameManager.UnlockTrojanHorse();

            pickupMessage.ShowMessage(
                "Trojan Horse ottenuto! Premere 'C' per attivare/disattivare"
            );

            Destroy(gameObject);
            return;
        }

        // Il collider appartiene a un NPC
        Nemico nemico = other.GetComponent<Nemico>();

        if (nemico != null && nemico.parassita != null)
        {
            parassita = nemico.parassita;

            if (parassita.corpoPosseduto == nemico.gameObject)
            {
                GameManager.gameManager.UnlockTrojanHorse();

                pickupMessage.ShowMessage(
                    "Trojan Horse ottenuto! Premere 'C' per attivare/disattivare"
                );

                Destroy(gameObject);
            }
        }
    }
}