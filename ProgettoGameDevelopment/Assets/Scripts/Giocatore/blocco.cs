using UnityEngine;

public class blocco : MonoBehaviour
{
    [SerializeField] private Collider2D Blocco;

    private void Start()
    {
        // Il blocco è attivo se NON abbiamo il Trojan Horse
       Blocco.enabled = true;
    }

    private void Update()
    {
        AggiornaBlocco();
    }

    private void AggiornaBlocco()
    {
        if (GameManager.gameManager == null)
            return;

        // SENZA Trojan Horse = blocco attivo
        // CON Trojan Horse = blocco disattivato
        Blocco.enabled = !GameManager.gameManager.hasTrojanHorse;
    }
}

