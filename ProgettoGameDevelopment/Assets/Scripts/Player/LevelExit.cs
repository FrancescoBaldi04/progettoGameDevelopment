using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private string nextScene;
    [SerializeField] private GameObject LevelMessage;
    [SerializeField] private GameObject BodyMessage;
    private Coroutine messageCoroutine;

    private void Start(){
        LevelMessage.SetActive(false);
        BodyMessage.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
         Enemy enemy = other.GetComponent<Enemy>();
        
            if(!GameManager.gameManager.hasWorm){
                 if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }

        messageCoroutine = StartCoroutine(ShowMessage());

                return;
            }
                if (other.GetComponent<Parasite>() != null){
                    SceneManager.LoadScene(nextScene);
                }
                    if(enemy!=null && enemy.currentState == Enemy.State.possessed){
                        if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }

        messageCoroutine = StartCoroutine(ShowMessage());
                    }

      
    }
    private IEnumerator ShowMessage()
    { if(!GameManager.gameManager.hasWorm){
        LevelMessage.SetActive(true);

        yield return new WaitForSeconds(3f);

        LevelMessage.SetActive(false);

        messageCoroutine = null;
    }else{
         BodyMessage.SetActive(true);

        yield return new WaitForSeconds(3f);

        BodyMessage.SetActive(false);

        messageCoroutine = null;

    }
    }
}