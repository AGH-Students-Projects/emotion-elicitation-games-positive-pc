using UnityEngine;

public class Heart : MonoBehaviour
{
    [SerializeField] private AudioClip CollectSound;

    private Animator HeartAnimator;

    private void Awake()
    {
        HeartAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerHealth.Instance.CurrentHealth != PlayerHealth.Instance.StartingHealth)
        {
            this.GetComponent<Collider2D>().enabled = false;
            HeartAnimator.SetTrigger("Collected");
            SoundManager.Instance.PlaySound(CollectSound);
            PlayerHealth.Instance.Heal(1);
            Destroy(gameObject, HeartAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.5f);
        }
    }
}
