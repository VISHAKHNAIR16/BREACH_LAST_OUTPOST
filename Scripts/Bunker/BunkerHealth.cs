using UnityEngine;

public class BunkerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth = Mathf.Max(currentHealth - amount, 0);
        Debug.Log("Bunker HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            OnBunkerDestroyed();
        }
    }

    private void OnBunkerDestroyed()
    {
        Debug.Log("Bunker destroyed! Night failed.");
        // TODO: show game over UI / restart
    }

    public int CurrentHealth => currentHealth;
}
