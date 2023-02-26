
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public Transform target;


    public Vector3 offset;

    private void LateUpdate()
    {


        transform.position = target.position + offset;
    }
}
