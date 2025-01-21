using UnityEngine;

public class EnemiesManager : MonoBehaviour
{
    [SerializeField] private MaleEnemyPatrol[] EnemyPatrols;

    private void Awake()
    {
        EnemyPatrols = GetComponentsInChildren<MaleEnemyPatrol>();

        DisableEnemies();
    }

    public void DisableEnemies()
    {
        foreach (var enemy in EnemyPatrols)
        {
            enemy.gameObject.SetActive(false);
        }
        Debug.Log("Enemies are disabled");
    }

    public void EnableEnemies()
    {
        foreach (var enemy in EnemyPatrols)
        {
            enemy.gameObject.SetActive(true);
        }
    }   
}
