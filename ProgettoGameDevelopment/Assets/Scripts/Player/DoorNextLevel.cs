using UnityEngine;

public class DoorNextLevel : MonoBehaviour
{
    private Animator Animator;
    private scientistMiniboss Miniboss;
    [SerializeField] private Collider2D DoorCollider;

    void Start()
    {
        Miniboss = FindFirstObjectByType<scientistMiniboss>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Miniboss==null) { // Open the door only if the MiniBoss has been defeated
            Animator.SetTrigger("Open");
            DoorCollider.enabled = false;
            enabled = false;
        }
    }
}
