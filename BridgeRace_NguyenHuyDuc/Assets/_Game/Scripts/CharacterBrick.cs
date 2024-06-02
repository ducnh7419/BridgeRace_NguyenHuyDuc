using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBrick : Brick
{
    internal void SetPosition(Vector3 position)
    {
         transform.position = position;
    }
}
