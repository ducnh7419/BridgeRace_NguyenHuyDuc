using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField]Vector3 offset = new Vector3(-3,10,-20);

    public Transform Target { get => target; set => target = value; }

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