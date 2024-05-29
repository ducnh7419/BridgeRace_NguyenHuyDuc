using System.Collections;
using System.Collections.Generic;
using GlobalEnum;
using UnityEngine;

public class Brick : GameUnit
{

    public ColorByEnum ColorByEnum;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ColorData colorData;


    public void SetBrickColor(int index)
    {
        ColorByEnum = (ColorByEnum)index;
        meshRenderer.material = colorData.GetColorByEnum(index);
    }

    IEnumerator EnableBrick()
    {
        yield return new WaitForSeconds(5f);
        SimplePool.Spawn(this);
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(GlobalConstants.Tag.PLAYER) || other.CompareTag(GlobalConstants.Tag.BOT))
        {
            Character character = Cache.GetCharacter(other);
            if (character.ColorByEnum == ColorByEnum)
            {
                {
                    Brick brickGO = SimplePool.Spawn<Brick>(this, character.brickBag.position,transform.rotation);
                    brickGO.transform.SetParent(character.transform);
                    brickGO.transform.localRotation=transform.localRotation;
                    brickGO.SetBrickColor((int)ColorByEnum);
                    
                    character.brickBag.position += new Vector3(0, 0.3f, 0);
                    SimplePool.Despawn(this);
                }
            }
        }


    }
}
