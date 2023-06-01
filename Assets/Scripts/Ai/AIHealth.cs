using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealth : MonoBehaviour, IDataPersistence, IDamageable
{
    public float health = 200f; // set the initial health of the enemy
    public Animator animator; 

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) KillEnemy();
    }

    public void KillEnemy() {
                animator.SetTrigger("Die"); // Play the death animation
                GetComponent<Collider>().enabled = false; // disable the collider
                GetComponent<AIController>().Stop(); // stop the enemy from moving
                Invoke("DisableEnemy", 10f); // disable the enemy object after 10 seconds
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            health -= 50f; // decrease the enemys health by 10
            if (health <= 0) 
            {
                KillEnemy();
            }
        }
    }

    private void DisableEnemy()
    {
        gameObject.SetActive(false);
    }

    public void LoadData(GameData data)
    {
        // Disable this enemy if it is in the list of killed enemies
        if (data.killedEnemies.Contains(gameObject.name))
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData(ref GameData data)
    {
        // Add this enemy to the list of killed enemies if its health is 0 or less
        if (health <= 0)
        {
            data.killedEnemies.Add(gameObject.name);
        }
    }
}
