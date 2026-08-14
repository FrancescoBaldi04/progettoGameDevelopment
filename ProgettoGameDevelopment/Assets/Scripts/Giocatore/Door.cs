using UnityEngine;
using UnityEngine.InputSystem; 

public class Door : MonoBehaviour
{
    private Animator animator;
    private bool playerNearby = false;
    [SerializeField] private Collider2D doorCollider;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerNearby && Keyboard.current.oKey.wasPressedThisFrame)
        {
            animator.SetTrigger("Open");
            doorCollider.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    { 

    if (other.CompareTag("Player"))
    {
        playerNearby = true;
    }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            if (doorCollider.enabled == false)
        {
            animator.SetTrigger("Close");
            doorCollider.enabled = true;
        }
        }
    }
    
}