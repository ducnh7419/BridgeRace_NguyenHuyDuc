using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private static CameraFollow ins;
    public static CameraFollow Ins => ins;

    private Transform target;
    [SerializeField] private float smoothSpeed;
    [SerializeField]Vector3 offset = new Vector3(-3,10,-20);
    Vector3 defaultCamOffset;

    public Transform Target { get => target; set => target = value; }

    private void Awake(){
        if (ins != null && ins != this)
        {
            Destroy(this);
        }
        else
        {
            ins = this;
        }
        defaultCamOffset=offset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null){
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    public void SetWinCam(){
        offset=new Vector3(2,7,-20);
    }

    public void ResetCam(){
        offset = defaultCamOffset;
    }
}