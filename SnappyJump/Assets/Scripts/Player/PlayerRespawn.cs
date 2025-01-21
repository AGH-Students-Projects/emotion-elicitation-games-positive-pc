using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Sounds")]
    [SerializeField] private AudioClip CheckpointSound;
    [SerializeField] private AudioClip FinishSound;
    [SerializeField] private AudioClip StartSound;

    private Transform CurrentCheckpoint;

    public void CheckRespawn()
    {
        if (CurrentCheckpoint == null || PlayerHealth.Instance.Lives == 0)
        {
            UIManager.Instance.BuyLiveUI.SetActive(true);
            return;
        }

        PlayerHealth.Instance.Respawn();
    }

    public void MovePlayerToRespawnPosition()
    {
        transform.position = CurrentCheckpoint.position;

        var inputActions = PlayerInputManager.Instance.InputActions;
        inputActions.Gameplay.Enable();
    }
}

