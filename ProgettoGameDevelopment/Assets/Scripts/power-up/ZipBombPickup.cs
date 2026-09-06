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
        Parasite parasite = other.GetComponent<Parasite>();

        // Parassita libero
        if (parasite != null)
        {
            GameManager.gameManager.UnlockZipBomb();

            if (PauseManager.pauseManager != null)
            {
                PauseManager.pauseManager.UpdateZipBombText();
            }

            pickupMessage.ShowMessage("Zip Bomb obtained! While controlling an NPC, Press 'E' to activate");

            Destroy(gameObject);
            return;
        }

        // Il collider appartiene a un NPC
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null && enemy.parasite != null)
        {
            parasite = enemy.parasite;

            if (parasite.possessedBody == enemy.gameObject)
            {
                GameManager.gameManager.UnlockZipBomb();

                if (PauseManager.pauseManager != null)
                {
                    PauseManager.pauseManager.UpdateZipBombText();
                }

                pickupMessage.ShowMessage("Zip Bomb obtained! While controlling an NPC, Press 'E' to activate");

                Destroy(gameObject);
            }
        }
    }
}