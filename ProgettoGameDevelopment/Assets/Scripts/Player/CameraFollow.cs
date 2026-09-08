using UnityEngine;
using Cinemachine; 

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow instance { get; private set;} // Singleton

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake()
    { 
        if (instance == null) // Singleton Pattern
        {
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }        
    }

    public void SetTarget(Transform newTarget)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = newTarget;
        }
    }
}