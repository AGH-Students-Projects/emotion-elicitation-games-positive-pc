using UnityEngine;

public class TreasureScript : MonoBehaviour
{
    [Header("Treasure Values")]
    [SerializeField] private int TreasureValueDiamonds = 0;
    [SerializeField] private int TreasureValueCoins = 0;

    [Header("Sounds")]
    [SerializeField] private AudioClip OpenSound;
    [SerializeField] private AudioClip NoKeysSound;

    private Animator TreasureAnimator;
    private bool IsOpened;

    private void Awake()
    {
        TreasureAnimator = GetComponent<Animator>();
        IsOpened = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (CanBeOpened())
            {
                IsOpened = true;
                this.GetComponent<Collider2D>().enabled = false;
                TreasureAnimator.SetTrigger("Open");
                SoundManager.Instance.PlaySound(OpenSound);

                // Add and loose resources
                PlayerResources.Instance.SetKeys(PlayerResources.Instance.Keys - 1);
                PlayerResources.Instance.SetDiamonds(PlayerResources.Instance.Diamonds + TreasureValueDiamonds);
                PlayerResources.Instance.SetCoins(PlayerResources.Instance.Coins + TreasureValueCoins);

                // Destroy the object after the animation is done
                Destroy(gameObject, TreasureAnimator.GetCurrentAnimatorStateInfo(0).length);
            } else 
            {
                TreasureAnimator.SetTrigger("ShowDialogBox");
                SoundManager.Instance.PlaySound(NoKeysSound);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TreasureAnimator.SetTrigger("HideDialogBox");
        }
    }

    private bool CanBeOpened()
    {
        return !IsOpened && PlayerResources.Instance.Keys > 0;
    }
}
