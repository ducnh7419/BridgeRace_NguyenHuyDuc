using System;
using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using Unity.VisualScripting;
using UnityEngine;

public class Brick : GameUnit
{

    public ColorByEnum ColorByEnum;
    [SerializeField] private MeshRenderer meshRenderer;
     [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ColorData colorData;


    public void SetBrickColor(int index)
    {
        ColorByEnum = (ColorByEnum)index;
        meshRenderer.material = colorData.GetColorByEnum(index);
    }

    internal void TurnOff()
    {
        meshRenderer.enabled = false;   
        boxCollider.enabled = false;
        StartCoroutine(CoDelayReappear());
    }

    internal void TurnOn()
    {
        meshRenderer.enabled = true;   
        boxCollider.enabled = true;
    }

    private IEnumerator CoDelayReappear() {
        yield return new WaitForSeconds(3);
        TurnOn();
    }

    IEnumerator EnableBrick()
    {
        yield return new WaitForSeconds(5f);
        SimplePool.Spawn(this);
    }

    public void Despawn(){
       SimplePool.Despawn(this);
    }
}
