using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healplayer : MonoBehaviour
{
    public playerhealth playerHealth;

    public void healplayernpc()
    {

        playerHealth.current_health = playerHealth.max_health;
        playerHealth.healthbar.SetHealth(playerHealth.current_health);

    }
}
