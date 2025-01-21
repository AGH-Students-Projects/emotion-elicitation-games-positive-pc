using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool isFinish = false;

    public AudioClip checkpointSound;
    public AudioClip finishSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isFinish)
            {
                SoundManager.Instance.PlaySound(finishSound);

                // Disable trigger collider
                GetComponent<BoxCollider2D>().enabled = false;

                // Load next level
                GameManager.Instance.LoadNextLevel();
            }
            else
            {
                RespawnManager.Instance.SetCheckpoint(transform.position);
            }
        }
    }
}
