using UnityEngine;

public class TrojanHorsePickup : MonoBehaviour
{
    private PickupMessage pickupMessage;

    private void Start()
    {
       pickupMessage = FindFirstObjectByType<PickupMessage>(FindObjectsInactive.Include);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        Parasite parasite = other.GetComponent<Parasite>();

        if (parasite != null)
        {
            GameManager.gameManager.UnlockTrojanHorse();

            if (PauseManager.pauseManager != null)
            {
                PauseManager.pauseManager.UpdateTrojanHorseText();
            }

            pickupMessage.ShowMessage("Trojan Horse obtained!");

            Destroy(gameObject);
            return;
        }

        
        Enemy enemy = other.GetComponent<Enemy>();

        if (enemy != null && enemy.parasite != null)
        {
            parasite = enemy.parasite;

            if (parasite.possessedBody == enemy.gameObject)
            {
                GameManager.gameManager.UnlockTrojanHorse();

                if (PauseManager.pauseManager != null)
                {
                    PauseManager.pauseManager.UpdateTrojanHorseText();
                }

                pickupMessage.ShowMessage("Trojan Horse obtained!");

                Destroy(gameObject);
            }
        }
    }
}