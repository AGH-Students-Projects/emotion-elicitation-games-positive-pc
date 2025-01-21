using UnityEngine;

public class BridgeStoneBall : MonoBehaviour
{
    [SerializeField] private AudioClip DestroySound;

    public bool CollidedWithPlayer { get; private set; }
    public float BallCollapseTime = 0.7f;

    private Animator BallAnimator;

    void Start()
    {
        BallAnimator = GetComponent<Animator>();
        CollidedWithPlayer = false;
    }

    void Update()
    {
        if (CollidedWithPlayer)
        {
            BallCollapseTime -= Time.deltaTime;

            if (BallCollapseTime <= 0)
            {
                //SoundManager.Instance.PlaySound(DestroySound);
                BallAnimator.SetTrigger("Destroy");
                Destroy(gameObject, BallAnimator.GetCurrentAnimatorStateInfo(0).length);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            CollidedWithPlayer = true;
    }
}
