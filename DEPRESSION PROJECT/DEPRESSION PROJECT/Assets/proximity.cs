using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximity : MonoBehaviour
{
    public GameObject uiElement;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("detect");

        if (other.tag == "player")
        {
            uiElement.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            uiElement.SetActive(false);
        }
    }
}