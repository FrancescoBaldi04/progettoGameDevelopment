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
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Parasite parasite = other.GetComponent<Parasite>();
        Enemy enemy = other.GetComponent<Enemy>();

        // Check if collision comes from the Parasite or a possessed NPC
        if (parasite != null || (enemy != null && enemy.currentState == Enemy.State.possessed))
        {
            if(!GameManager.gameManager.hasTrojanHorse){
                if (messageCoroutine != null)
                {
                    StopCoroutine(messageCoroutine);
                }

                messageCoroutine = StartCoroutine(ShowMessage());
            }
            else
            {
                block.enabled = false; // Disable physical barrier
            }
        }
    }

    private IEnumerator ShowMessage() // Display the message for 3 seconds
    {
        BlockMessage.SetActive(true);
        yield return new WaitForSeconds(3f);

        BlockMessage.SetActive(false);
        messageCoroutine = null;
    }
}