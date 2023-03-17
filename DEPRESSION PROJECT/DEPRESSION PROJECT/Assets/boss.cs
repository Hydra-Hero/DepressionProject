using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss : MonoBehaviour
{

    public Animator anim;

    public Transform attack_point;
    public float attack_range = 0.5f;
    public LayerMask playerLayers;
    public int attack_damage = 40;
    public float attack_speed = 2f;
    private float next_attack_time = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= next_attack_time)
        {

            Attack();
            next_attack_time = Time.time + 1f / attack_speed;

        }


    }



    private void Attack()
    {
        //play attack anim
        anim.SetTrigger("attacking");

        Collider2D[] hit_player = Physics2D.OverlapCircleAll(attack_point.position, attack_range, playerLayers);

        foreach (Collider2D player in hit_player)
        {

            player.GetComponent<playerhealth>().TakeDamage(attack_damage);

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
