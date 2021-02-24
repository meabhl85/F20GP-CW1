using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Health variables
    public int maxHealth = 100;
    public int currentHealth;

    //References to UI 
    public HealthBar healthBar;
    public Score score;

    // Start is called before the first frame update
    void Start()
    {
        //Setting initial values
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage)
    {
        //Update health
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        //Death Check
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        //Pass on final score to end screen
        int endScore = score.getScore();
        FindObjectOfType<GameManager>().EndGame(endScore);
    }
}
