using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int max_health = 100;
    public int current_health;
    public float knock_back_force = 2.0f;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
    }

    public void TakeDamage(int Damage)
    {

        anim.SetTrigger("Hit");

        current_health -= Damage;

        //hurt anim

        if (current_health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("died");

        anim.SetBool("isDead", true);

        StartCoroutine(DisableAfterAnimation());
    }

    private IEnumerator DisableAfterAnimation()
    {
        yield return new WaitForSeconds(2.30f);
        gameObject.SetActive(false);
    }


}
