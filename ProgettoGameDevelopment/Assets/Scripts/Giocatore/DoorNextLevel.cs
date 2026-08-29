using UnityEngine;

public class DoorNextLevel : MonoBehaviour
{
     private Animator animator;
     private scientistMiniboss miniboss;
     [SerializeField] private Collider2D doorCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         miniboss = FindFirstObjectByType<scientistMiniboss>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
         if (miniboss==null)
        {
            animator.SetTrigger("Open");
            doorCollider.enabled = false;
             enabled = false;
        }
    }



}
