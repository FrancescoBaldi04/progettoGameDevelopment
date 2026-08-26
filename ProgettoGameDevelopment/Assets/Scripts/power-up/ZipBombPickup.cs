using UnityEngine;

public class ZipBombPickup : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Parassita parassita = other.GetComponent<Parassita>();
        //  Parassita libero
        if (parassita != null)
        {
            GameManager.gameManager.UnlockZipBomb();

            Destroy(gameObject);
        }
        Nemico nemico = other.GetComponent<Nemico>();
        //  il collider appartiene a un NPC
        if (nemico != null && nemico.parassita != null)
        {
            parassita = nemico.parassita;

            
            if (parassita.corpoPosseduto == nemico.gameObject)
            {
                GameManager.gameManager.UnlockZipBomb();
                Destroy(gameObject);
            }
        }
    }
}
