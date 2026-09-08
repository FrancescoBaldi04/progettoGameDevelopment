using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
            // Start the animation
            animator.SetTrigger("Open");

            // Disable the collider after 1 second
            StartCoroutine(DisableColliderAfterDelay());
        }
    }

    private IEnumerator DisableColliderAfterDelay()
    {
        yield return new WaitForSeconds(0.25f);

        doorCollider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Allow the door to open when the player possesses a scientist
        Scientist scientist = other.GetComponent<Scientist>();

        if (scientist != null && scientist.currentState == Scientist.State.possessed)
        {
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Close the door when the possessed scientist leaves the trigger
        Scientist scientist = other.GetComponent<Scientist>();

        if (scientist != null && scientist.currentState == Scientist.State.possessed)
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
