using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlscreenscript : MonoBehaviour
{
    public GameObject objectToDestroy;
    public GameObject objectToEnable;

    void Start()
    {
        // Destroy objectToDestroy after 5 seconds
        Destroy(objectToDestroy, 5f);
    }

    void OnDestroy()
    {
        // Enable objectToEnable when objectToDestroy is destroyed
        objectToEnable.SetActive(true);
    }
}
