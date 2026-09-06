using UnityEngine;
using System.Collections;

public class blocco : MonoBehaviour
{
    [SerializeField] private Collider2D Blocco;
    [SerializeField] private GameObject BlockMessage;

    private Coroutine messageCoroutine;

    private void Start()
    {
        BlockMessage.SetActive(false);
        AggiornaBlocco();
    }

    private void Update()
    {
        AggiornaBlocco();
    }

    private void AggiornaBlocco()
    {
        if (GameManager.gameManager == null)
            return;

        if (GameManager.gameManager.hasTrojanHorse)
        {
            Blocco.enabled = false;
        }
        else
        {
            Blocco.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.gameManager == null)
            return;

        if (GameManager.gameManager.hasTrojanHorse)
            return;

        if (other.GetComponent<Parassita>() != null)
        {
            if (messageCoroutine != null)
                StopCoroutine(messageCoroutine);

            messageCoroutine = StartCoroutine(ShowMessage());
        }
    }

    private IEnumerator ShowMessage()
    {
        BlockMessage.SetActive(true);

        yield return new WaitForSeconds(3f);

        BlockMessage.SetActive(false);

        messageCoroutine = null;
    }
}