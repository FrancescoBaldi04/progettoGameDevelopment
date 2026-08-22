using UnityEngine;
using Cinemachine; 

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        instance = this;
    }

    public void SetTarget(Transform newTarget)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = newTarget;
        }
    }
}