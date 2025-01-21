using UnityEngine;

public class PlatformMechanics : MonoBehaviour
{

    [SerializeField] private float PlatformLiveSpan;
    [SerializeField] private float Delay = 0.1f;
    private float CollideWithPlayer;
    private bool Colided;

    private Animator PlatformAnimator;
    private BoxCollider2D PlatformCollider;

    void Awake()
    {
        Colided = false;
        PlatformAnimator = GetComponent<Animator>();
        PlatformCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (Colided)
        {
            CollideWithPlayer += Time.deltaTime;
            if (CollideWithPlayer >= PlatformLiveSpan)
            {
                PlatformAnimator.SetTrigger("Destroy");
                Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + Delay);
                PlatformCollider.enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Colided = true;
    }
}
