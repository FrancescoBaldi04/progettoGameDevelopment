using UnityEngine;

public class blocco : MonoBehaviour
{  
    [SerializeField] private Collider2D Blocco;
    private bool playerNearby = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { if (playerNearby ){
       Blocco.enabled = false;
    }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    { 

   Nemico nemico = other.GetComponent<Nemico>();

if (nemico != null && nemico.StatoAttuale == Nemico.Stato.possessed && GameManager.gameManager.hasTrojanHorse)
    {
        playerNearby = true;
    }
    }
}
