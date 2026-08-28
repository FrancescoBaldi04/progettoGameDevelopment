using UnityEngine;

public class ZipBombPickup : MonoBehaviour
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
        Parassita parassita = other.GetComponent<Parassita>();

        // Parassita libero
        if (parassita != null)
        {
            GameManager.gameManager.UnlockZipBomb();

            pickupMessage.ShowMessage(
                "Zip Bomb ottenuta! Premere 'E' per attivare"
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
                GameManager.gameManager.UnlockZipBomb();

                pickupMessage.ShowMessage(
                    "Zip Bomb ottenuta! Premere 'C' per attivare/disattivare"
                );

                Destroy(gameObject);
            }
        }
    }
}