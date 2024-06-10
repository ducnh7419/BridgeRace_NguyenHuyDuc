using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using Unity.VisualScripting;
using UnityEngine;

public class DroppedBrick : GameUnit
{

    public ColorByEnum ColorByEnum;
    [SerializeField] private ColorData colorData;

    internal void SetPosition(Vector3 position)
    {
         transform.position = position;
    }

    internal void SetRotation(Quaternion rot)
    {
         transform.rotation = rot;
    }

    

}
