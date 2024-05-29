using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class Stair : GameUnit
{
    public ColorByEnum ColorByEnum;

    [SerializeField] private ColorData colorData;

    [SerializeField] MeshRenderer meshRenderer;
    

    public void SetStairColor(int index){
        ColorByEnum = (ColorByEnum)index;
        meshRenderer.material = colorData.GetColorByEnum(index);
    }
    
}
