using UnityEngine;

public class BottomTrap : MonoBehaviour
{
    [Header("Trap Damage")]
    [SerializeField] private float TrapDamage = 3;

    [Header("Sound")]
    [SerializeField] private AudioClip TrapSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SoundManager.Instance.PlaySound(TrapSound);
            collision.GetComponent<PlayerHealth>().TakeDamage(TrapDamage);
        }
    }
}
