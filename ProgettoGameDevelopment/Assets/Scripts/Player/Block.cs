using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour
{
    [SerializeField] private Collider2D block;
    [SerializeField] private GameObject BlockMessage;

    private Coroutine messageCoroutine;

    private void Start()
    {
        BlockMessage.SetActive(false);
        BlockUpdate();
    }

    private void Update()
    {
        BlockUpdate();
    }

    private void BlockUpdate()
    {
        if (GameManager.gameManager == null)
            return;

        if (GameManager.gameManager.hasTrojanHorse)
        {
            block.enabled = false;
        }
        else
        {
            block.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (GameManager.gameManager == null)
            return;

        if (GameManager.gameManager.hasTrojanHorse)
            return;

        if (other.GetComponent<Parasite>() != null)
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