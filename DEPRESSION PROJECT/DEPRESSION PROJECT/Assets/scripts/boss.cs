using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{
    public float meleeRange = 2f; // Distance at which the boss can do a melee attack
    public float summonRange = 20f;
    public float meleeCooldown = 5f; // Cooldown time for the melee attack
    public GameObject summonPrefab; // Prefab for the enemy sprite
    public int numEnemies = 10; // Number of enemy sprites to spawn
    public float waveCooldown = 5f; // Cooldown time between waves
    public Transform[] spawnPoints; // List of spawn points for the enemy sprites
    public Transform attack_point;
    public float maxHealth = 100f; // Maximum health of the boss
    public float currentHealth; // Current health of the boss
    private List<GameObject> summons; // List to store enemy sprites
    private bool canAttack = true; // Whether the boss can currently do a melee attack
    private float cooldownTimer = 0f; // Timer for the melee attack cooldown
    private int remainingEnemies = 0; // Number of enemies remaining in the current wave
    private float waveTimer = 0f; // Timer for the wave cooldown
    public LayerMask playerlayer;
    private Animator animator; // Reference to the animator component
    public int attack_damage = 50;
    public GameObject hidden_passage;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        // Initialize summons list
        summons = new List<GameObject>();

        // Set up initial summon range
        float initialSummonRange = 10f;

        // Get the position of the player
        Vector2 playerPos = GameObject.FindGameObjectWithTag("player").transform.position;

        // Check if the player is within the initial summon range
        if (Vector2.Distance(transform.position, playerPos) < initialSummonRange)
        {
            // Spawn the initial wave of summons
            for (int i = 0; i < spawnPoints.Length; i++)
            {
                GameObject summon = Instantiate(summonPrefab, spawnPoints[i].position, Quaternion.identity);
                summon.GetComponent<summonmove>().SetBoss(gameObject);
                summons.Add(summon);
            }
            remainingEnemies = spawnPoints.Length;
        }

        // Set up the new summon range
        float newSummonRange = 20f;

        // Set up a coroutine to periodically check if the player is within the new summon range
        StartCoroutine(CheckSummonRange(newSummonRange));
    }

    void Update()
    {
        // Get the position of the player
        Vector3 playerPos = GameObject.FindGameObjectWithTag("player").transform.position;

        // Check if the player is on the left or right side
        if (playerPos.x < transform.position.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        // Check if the player is close enough to do a melee attack
        if (Vector2.Distance(transform.position, playerPos) < meleeRange)
        {
            if (canAttack)
            {
                Attack();
            }
        }

        // Check if the cooldown is over and enable the boss's attack ability
        if (!canAttack)
        {
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0f)
            {
                canAttack = true;
            }
        }

        // Check if all enemies in the current wave have been defeated and if the wave cooldown is over
        if (remainingEnemies <= 0)
        {
            waveTimer -= Time.deltaTime;
            if (waveTimer <= 0f)
            {
                // Check if the player is within summon range
                if (Vector2.Distance(transform.position, playerPos) < summonRange)
                {
                    // Start a new wave
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        GameObject summon = Instantiate(summonPrefab, spawnPoints[i].position, Quaternion.identity);
                        summon.GetComponent<summonmove>().SetBoss(gameObject);
                        summons.Add(summon);
                    }
                    remainingEnemies = spawnPoints.Length;
                }
                waveTimer = waveCooldown;
            }
        }
    }

    IEnumerator CheckSummonRange(float range)
    {
        while (true)
        {
            // Get the position of the player
            Vector2 playerPos = GameObject.FindGameObjectWithTag("player").transform.position;

            // Check if the player is within the new summon range
            if (Vector2.Distance(transform.position, playerPos) < range)
            {
                // Check if the summons have already been spawned
                if (summons.Count == 0)
                {
                    // Spawn the summons
                    for (int i = 0; i < spawnPoints.Length; i++)
                    {
                        GameObject summon = Instantiate(summonPrefab, spawnPoints[i].position, Quaternion.identity);
                        summon.GetComponent<summonmove>().SetBoss(gameObject);
                        summons.Add(summon);
                    }
                    remainingEnemies = spawnPoints.Length;
                }
            }
            else
            {
                // Remove any existing summons
                foreach (GameObject summon in summons)
                {
                    Destroy(summon);
                }
                summons.Clear();
                remainingEnemies = 0;
            }

            // Wait for a short time before checking again
            yield return new WaitForSeconds(2f);
        }
    }
    // Called by enemy sprites when they are defeated
    public void EnemyDefeated()
    {
        // Decrement the remaining enemy count
        remainingEnemies--;
    }

    // Called by the player's attack
    public void TakeDamage(Collider2D collider, int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hit");
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            animator.SetBool("isdead", true);
            Destroy(gameObject, 2f);
            Destroy(hidden_passage);
        }
    }


    private void Attack()
    {
        //play attack anim
        animator.SetTrigger("attack");
        Debug.Log("Boss did a melee attack!");
        canAttack = false;
        cooldownTimer = meleeCooldown;

        Collider2D[] hit_enemies = Physics2D.OverlapCircleAll(attack_point.position, meleeRange, playerlayer);

        foreach (Collider2D enemy in hit_enemies)
        {
            if (enemy.GetComponent<boss>() != null)
            {
                enemy.GetComponent<boss>().TakeDamage(GetComponent<Collider2D>(), attack_damage);
            }

            // Check if the enemy hit is the player and within range, then damage the player
            GameObject player = GameObject.FindGameObjectWithTag("player");
            if (enemy.gameObject == player && Vector2.Distance(transform.position, player.transform.position) < meleeRange)
            {
                player.GetComponent<playerhealth>().TakeDamage(attack_damage);
            }

            Debug.Log("hit " + enemy.name);
        }
    }

    private void OnDrawGizmosSelected()
    {

        if (attack_point == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attack_point.position, meleeRange);
        Gizmos.DrawWireSphere(attack_point.position, summonRange);
    }


}
