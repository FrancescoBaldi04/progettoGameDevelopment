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

        if (parassita != null)
        {
            GameManager.gameManager.UnlockZipBomb();

            Destroy(gameObject);
        }
        Nemico nemico = other.GetComponent<Nemico>();

        if (nemico != null && nemico.parassita != null)
        {
            parassita = nemico.parassita;

            GameManager.gameManager.UnlockZipBomb();

            Destroy(gameObject);
            }
    }
}
