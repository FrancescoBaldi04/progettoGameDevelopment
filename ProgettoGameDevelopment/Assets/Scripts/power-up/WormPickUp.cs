using UnityEngine;

public class WormPickUp : MonoBehaviour
{
    [SerializeField] private string wormUIdescription;
    private PickupMessage pickupMessage;

    private void Start()
    {
        pickupMessage = FindFirstObjectByType<PickupMessage>(FindObjectsInactive.Include);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Parassita parassita = other.GetComponent<Parassita>();

        if (parassita != null)
        {
            GameManager.gameManager.UnlockWorm();

            if (PauseManager.pauseManager != null)
            {
                PauseManager.pauseManager.UpdateWormText();
            }

            pickupMessage.ShowMessage("Worm ottenuto! Premere 'C' per attivare/disattivare");

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

                if (PauseManager.pauseManager != null)
                {
                    PauseManager.pauseManager.UpdateWormText();
                }

                pickupMessage.ShowMessage("Worm ottenuto! Premere 'C' per attivare/disattivare");

                Destroy(gameObject);
            }
        }
    }
}