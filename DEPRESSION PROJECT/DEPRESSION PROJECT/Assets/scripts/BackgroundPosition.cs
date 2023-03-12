using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundPosition : MonoBehaviour
{
    public GameObject caveBackground;
    public GameObject dungeonBackground;
    public GameObject strongholdBackground;
    public GameObject snowBackground;
    public GameObject forestBackground;

    public Transform playerTransform;

    void Update()
    {
        float playerY = playerTransform.position.y;

        // Disable or enable cave background based on player position
        if (playerY < -185)
        {
            caveBackground.SetActive(true);
        }
        else
        {
            caveBackground.SetActive(false);
        }

        // Disable or enable dungeon background based on player position
        if (playerY >= -185 && playerY < -125)
        {
            dungeonBackground.SetActive(true);
        }
        else
        {
            dungeonBackground.SetActive(false);
        }

        // Disable or enable stronghold background based on player position
        if (playerY >= -125 && playerY < -45)
        {
            strongholdBackground.SetActive(true);
        }
        else
        {
            strongholdBackground.SetActive(false);
        }

        // Disable or enable snow background based on player position
        if (playerY >= -45 && playerY < 12)
        {
            snowBackground.SetActive(true);
        }
        else
        {
            snowBackground.SetActive(false);
        }

        // Disable or enable forest background based on player position
        if (playerY >= 12)
        {
            forestBackground.SetActive(true);
        }
        else
        {
            forestBackground.SetActive(false);
        }
    }
}
