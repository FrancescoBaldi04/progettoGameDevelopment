using UnityEngine;
using TMPro;
using System.Collections;

public class PickupMessage : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float messageDuration = 3f;

    private Coroutine messageCoroutine;

    private void Start()
    {
        text.gameObject.SetActive(false);
    }

    public void ShowMessage(string message)
    {
        
        

        text.text = message;
        text.gameObject.SetActive(true);

        messageCoroutine = StartCoroutine(HideMessage());
    }

    private IEnumerator HideMessage()
    {
        yield return new WaitForSeconds(messageDuration);

        text.gameObject.SetActive(false);
        messageCoroutine = null;
    }
}