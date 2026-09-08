using UnityEngine;

public class WormPickUp : MonoBehaviour
{
    private PickupMessage pickupMessage;

    private void Start()
    {
        pickupMessage = FindFirstObjectByType<PickupMessage>(FindObjectsInactive.Include);
    }

    private void OnTriggerEnter2D(Collider2D other) // Unlock Worm
    {
        Parasite parasite = other.GetComponent<Parasite>();

        if (parasite != null)
        {
            GameManager.gameManager.UnlockWorm();

            if (PauseManager.pauseManager != null)
            {
                PauseManager.pauseManager.UpdateWormText();
            }

            pickupMessage.ShowMessage("Worm obtained! Press C to increase the parasite speed");
            Destroy(gameObject);
            return;
        }

        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null && enemy.parasite != null)
        {
            parasite = enemy.parasite;

            if (parasite.possessedBody == enemy.gameObject)
            {
                GameManager.gameManager.UnlockWorm();

                if (PauseManager.pauseManager != null)
                {
                    PauseManager.pauseManager.UpdateWormText();
                }

                pickupMessage.ShowMessage("Worm obtained! Press C to increase the parasite speed");

                Destroy(gameObject);
            }
        }
    }
}