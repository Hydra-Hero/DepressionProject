using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class playerhealth : MonoBehaviour
{
    public int max_health = 100;
    public int current_health;
    public playermovement playermovement;
    public playercombat playercombat;
    public Animator anim;
    public healthbar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
        healthbar.SetMaxHealth(max_health);
    }

    public void TakeDamage(int Damage)
    {

        anim.SetTrigger("hit");

        current_health -= Damage;
        healthbar.SetHealth(current_health);
        //hurt anim

        if (current_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("died");

        playermovement.enabled = false;
        playercombat.enabled = false;

        anim.SetBool("isDead", true);

        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        yield return new WaitForSeconds(3f);
        gameObject.SetActive(false);
    }
}