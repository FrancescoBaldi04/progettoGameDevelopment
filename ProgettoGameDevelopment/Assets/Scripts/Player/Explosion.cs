using UnityEngine;

public class Explosion : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) // Check if the current animation clip has finished playing
        {
            Destroy(gameObject);
        }
    }
}