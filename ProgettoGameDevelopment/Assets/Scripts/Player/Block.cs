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
    // Caso 1: entra direttamente il Parasite
    Parasite parasite = other.GetComponent<Parasite>();

    // Caso 2: entra un corpo posseduto
    Enemy enemy = other.GetComponent<Enemy>();

    bool parasiteEntered = parasite != null;

    bool possessedBodyEntered =
        enemy != null &&
        enemy.currentState == Enemy.State.possessed;

    if (parasiteEntered || possessedBodyEntered)
    {
        if(!GameManager.gameManager.hasTrojanHorse){
            StopCoroutine(messageCoroutine);

        messageCoroutine = StartCoroutine(ShowMessage());
        }else{
            block.enabled = false;
        }

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