using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{
    [SerializeField] private AudioClip CoinCollectSound;

    private Animator CoinAnimator;

    private void Awake()
    {
        CoinAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.GetComponent<Collider2D>().enabled = false;
            SoundManager.Instance.PlaySound(CoinCollectSound);
            PlayerResources.Instance.SetCoins(PlayerResources.Instance.Coins + 1);
            CoinAnimator.SetTrigger("Collected");
            Destroy(gameObject, CoinAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.5f);
        }
    }
}
