using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public Vector3 offset;

    public Transform p;
    private void Start()
    {
        offset = transform.position - p.position;
    }

    private void LateUpdate()
    {
        if (Manager.manager.enableCamFollowPlayer) {
            Vector3 v = p.position + offset;
            v.x = 0;

            transform.position = v;
        }
    }
}
