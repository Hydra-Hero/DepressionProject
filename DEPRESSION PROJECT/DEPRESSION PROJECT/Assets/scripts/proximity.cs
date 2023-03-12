using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proximity : MonoBehaviour
{
    public GameObject SpeechBubble;
    public GameObject TextBox;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("detect");

        if (other.tag == "player")
        {
            SpeechBubble.SetActive(true);
            TextBox.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "player")
        {
            SpeechBubble.SetActive(false);
            TextBox.SetActive(false);
        }
    }
}