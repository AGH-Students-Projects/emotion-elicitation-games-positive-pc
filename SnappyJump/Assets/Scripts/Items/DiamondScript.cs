using UnityEngine;

public class DiamondScript : MonoBehaviour
{
    [SerializeField] private AudioClip DiamondCollectSound;

    private Animator DiamondAnimator;

    void Start()
    {
        DiamondAnimator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            this.GetComponent<Collider2D>().enabled = false;
            DiamondAnimator.SetTrigger("Collected");
            SoundManager.Instance.PlaySound(DiamondCollectSound);
            PlayerResources.Instance.SetDiamonds(PlayerResources.Instance.Diamonds + 1);
            Destroy(gameObject, DiamondAnimator.GetCurrentAnimatorClipInfo(0).Length - 0.5f);
        }
    }
}
