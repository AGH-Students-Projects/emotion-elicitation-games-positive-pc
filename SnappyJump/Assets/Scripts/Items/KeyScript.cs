using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField] private AudioClip KeyCollectSound;

    private Animator KeyAnimator;

    void Start()
    {
        KeyAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.GetComponent<Collider2D>().enabled = false;
            SoundManager.Instance.PlaySound(KeyCollectSound);
            PlayerResources.Instance.SetKeys(PlayerResources.Instance.Keys + 1);
            KeyAnimator.SetTrigger("Collected");
            Destroy(gameObject, KeyAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.5f);
        }
    }
}
