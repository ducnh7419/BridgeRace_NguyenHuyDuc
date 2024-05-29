using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorData", menuName = "ScriptableObjects/ColorData", order = 1)]
public class ColorData : ScriptableObject
{
    public List<Material> colorsMaterial;
    
    public Material GetColorByEnum(int index){
        for(int i = 0;i<colorsMaterial.Count;i++){
            if(i==index){
                return colorsMaterial[i];
            }
        }
        return colorsMaterial[0];
    }
    
}
