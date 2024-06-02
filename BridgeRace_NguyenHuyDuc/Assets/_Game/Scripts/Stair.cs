using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class Stair : GameUnit
{
    public ColorByEnum ColorByEnum;

    [SerializeField] private ColorData colorData;

    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] private Collider brickCollider;
    

    public void SetStairColor(int index){
        ColorByEnum = (ColorByEnum)index;
        meshRenderer.material = colorData.GetColorByEnum(index);
    }

    public void  ActiveRenderer(){
        meshRenderer.enabled=true;
    }
    
}
