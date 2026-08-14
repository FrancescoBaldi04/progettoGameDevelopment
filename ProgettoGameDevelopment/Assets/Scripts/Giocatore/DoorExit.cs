using UnityEngine;

public class DoorExit : MonoBehaviour
{
    [SerializeField] private Collider2D doorCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            doorCollider.enabled = true;
            
        }
    }
}