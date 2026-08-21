using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField] private Transform target;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 offset = new Vector3(0, 0, -10f);
    private void Awake()
    {
        instance = this;
    }

    private void LateUpdate() // LateUpdate viene chiamato quando tutti i metodi Update di tutti gli script della scena sono stati completati
    {
        if (target == null) return;
        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
