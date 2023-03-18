using UnityEngine;
using System.Collections;

public class summonmove : MonoBehaviour
{
    public float speed = 5f; // Speed of the enemy sprite
    public int damage = 10; // Damage to be dealt on collision
    public Animator anim; // Animator component for animation
    private bool isDead = false; // Flag for whether the summon is dead or not
    private boss bossScript; // Reference to the boss script
    public float lifetime = 5f; // Lifetime of the summon
    public float floatiness = 0.5f;
    void Start()
    {
        // Get the boss script
        bossScript = GameObject.FindObjectOfType<boss>();

        // Trigger the spawn animation
        anim.SetTrigger("spawn");

        // Destroy the summon after a set time
        StartCoroutine(DestroyAfterDelay());
    }

    void Update()
    {
        // If the summon is dead, do nothing
        if (isDead) return;

        // Get a random direction
        Vector2 randomDirection = Random.insideUnitCircle.normalized;

        // Calculate the direction towards the player
        Vector2 directionToPlayer = (PlayerManager.instance.GetPlayerPosition() - transform.position).normalized;

        // Add a weighted random displacement to the direction towards the player
        Vector2 direction = (directionToPlayer * (1 - floatiness)) + (randomDirection * floatiness);

        // Normalize the final direction
        direction.Normalize();

        float distanceToMove = speed * Time.deltaTime;
        transform.Translate(direction * distanceToMove, Space.World);
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(lifetime);

        // Play the death animation and set the flag to true
        anim.SetBool("dead", true);
        isDead = true;

        // Notify the boss that the summon has been destroyed
        bossScript.EnemyDefeated();

        // Destroy the summon after the animation has finished playing
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // If the collider that collided with the summon is the player
        if (col.gameObject.CompareTag("player"))
        {
            // Deal damage to the player
            col.gameObject.GetComponent<playerhealth>().TakeDamage(damage);

            // Play the death animation and set the flag to true
            anim.SetBool("dead", true);
            isDead = true;

            // Notify the boss that the summon has been destroyed
            bossScript.EnemyDefeated();

            // Destroy the summon after the animation has finished playing
            Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
        }
    }

    public void SetBoss(GameObject bossObject)
    {
        bossScript = bossObject.GetComponent<boss>();
    }

    public void TakeDamage(int damage)
    {
        // Play the death animation and set the flag to true
        anim.SetBool("dead", true);
        isDead = true;

        // Notify the boss that the summon has been destroyed
        bossScript.EnemyDefeated();

        // Destroy the summon after the animation has finished playing
        Destroy(gameObject, anim.GetCurrentAnimatorStateInfo(0).length);
    }
}