using UnityEngine;

public class Friend : MonoBehaviour
{
    [SerializeField] private GameObject DialogBox;
    
    private Animator animator;
    

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("isJumping");
            DialogBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            DialogBox.SetActive(false);
        }
    }
}
