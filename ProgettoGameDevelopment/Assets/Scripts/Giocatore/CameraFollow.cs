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

    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition;

        SpriteRenderer spriteRenderer =
            target.GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            targetPosition = spriteRenderer.bounds.center;
        }
        else
        {
            targetPosition = target.position;
        }

        targetPosition += offset;

        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}