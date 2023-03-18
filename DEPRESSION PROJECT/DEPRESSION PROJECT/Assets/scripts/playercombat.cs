using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercombat : MonoBehaviour
{

    public Animator anim;

    public Transform attack_point;
    public float attack_range = 0.5f;
    public LayerMask enemyLayers;
    public int attack_damage = 40;
    public float attack_speed = 2f;
    private float next_attack_time = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= next_attack_time)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                next_attack_time = Time.time + 1f / attack_speed;
            }
        }


    }

    private void Attack()
    {
        //play attack anim
        anim.SetTrigger("attacking");

        Collider2D[] hit_enemies = Physics2D.OverlapCircleAll(attack_point.position, attack_range, enemyLayers);

        foreach (Collider2D enemy in hit_enemies)
        {
            if (enemy.GetComponent<boss>() != null)
            {
                enemy.GetComponent<boss>().TakeDamage(GetComponent<Collider2D>(), attack_damage);
            }
            else
            {
                enemy.GetComponent<enemy>().TakeDamage(attack_damage);
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

        Gizmos.DrawWireSphere(attack_point.position, attack_range);
    }

}
