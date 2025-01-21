using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Instance;

    private Vector3 RespawnPosition;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        RespawnPosition = GameObject.FindWithTag("Start").transform.position;
    }

    public void SetCheckpoint(Vector3 checkpointPosition)
    {
        RespawnPosition = checkpointPosition;
    }

    public void PlayerRespawn()
    {
        GameObject.FindWithTag("Player").transform.position = RespawnPosition;
    }

    public void InitializeRespawnPoint()
    {
        GameObject startPoint = GameObject.FindWithTag("Start");

        if (startPoint != null)
        {
            RespawnPosition = startPoint.transform.position;
        }
        else
        {
            Debug.LogError("Start point not found in the scene! Make sure there is a GameObject tagged as 'Start'.");
        }
    }

}
