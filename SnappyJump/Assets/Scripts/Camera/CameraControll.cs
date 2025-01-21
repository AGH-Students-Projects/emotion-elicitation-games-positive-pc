using UnityEngine;
using Cinemachine;
using System.Collections;

public class CameraControll : MonoBehaviour
{
    public static CameraControll Instance;

    public CinemachineVirtualCamera VirtualCamera { get; private set; }
    public CinemachineConfiner2D Confiner { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist the Cameras prefab across scenes
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }

        if (VirtualCamera == null)
        {
            VirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
        }

        if (Confiner == null)
        {
            Confiner = VirtualCamera.GetComponent<CinemachineConfiner2D>();
        }
    }

    private void Start()
    {
        SetupCameraForNewLevel();
    }

    public void SetupCameraForNewLevel()
    {
        SetFollowTarget();
        SetBoundingShape();
    }

    private void SetFollowTarget()
    {
        VirtualCamera.Follow = GameObject.FindWithTag("Player").transform;
    }

    private void SetBoundingShape()
    {

        GameObject levelBounds = GameObject.FindWithTag("LevelBounds");

        if (levelBounds == null)
        {
            Debug.LogError("LevelBounds not found! Ensure a GameObject with the 'LevelBounds' tag exists in the scene.");
        }

        PolygonCollider2D collider = levelBounds.GetComponent<PolygonCollider2D>();


        Confiner.m_BoundingShape2D = collider;
    }
}

