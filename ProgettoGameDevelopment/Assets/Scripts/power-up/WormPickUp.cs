using UnityEngine;

public class WormPickUp : MonoBehaviour
{
    private PickupMessage pickupMessage;

    private void Start()
    {
         pickupMessage = FindFirstObjectByType<PickupMessage>(
        FindObjectsInactive.Include
    );
        
         if (pickupMessage == null)
    {
        Debug.LogError("ERRORE: PickupMessage non trovato nella scena!");
    }
    else
    {
        Debug.Log("PickupMessage trovato correttamente!");
    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Parassita parassita = other.GetComponent<Parassita>();

        if (parassita != null)
        {
            GameManager.gameManager.UnlockWorm();

            pickupMessage.ShowMessage(
                "Worm ottenuto! Premere 'C' per attivare/disattivare"
            );

            Destroy(gameObject);
            return;
        }

        Nemico nemico = other.GetComponent<Nemico>();

        if (nemico != null && nemico.parassita != null)
        {
            parassita = nemico.parassita;

            if (parassita.corpoPosseduto == nemico.gameObject)
            {
                GameManager.gameManager.UnlockWorm();

                pickupMessage.ShowMessage(
                    "Worm ottenuto! Premere 'C' per attivare/disattivare"
                );

                Destroy(gameObject);
            }
        }
    }
}