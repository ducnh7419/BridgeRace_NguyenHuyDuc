using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField]Vector3 offset = new Vector3(-3,10,-20);

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null){
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}