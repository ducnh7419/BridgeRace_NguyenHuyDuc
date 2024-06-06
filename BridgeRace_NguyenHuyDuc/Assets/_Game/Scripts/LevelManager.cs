using System.Collections;
using System.Collections.Generic;
using System.Data;
using GlobalEnum;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager ins;
    public static LevelManager Ins => ins;
    public ColorData ColorData;
    public CameraFollow cameraFollow;
    public DynamicJoystick joystick;
    [SerializeField] private List<Level> level;
    private int currentLevel=0;
    public int Rank=0;

    private void Awake()
    {
        if (ins != null && ins != this)
        {
            Destroy(this);
        }
        else
        {
            ins = this;
        }
        
    }

    private void Start(){
        
    }

    public void GenerateLevel(){
        currentLevel=PlayerDataManager.CurrentLevel;
        Instantiate(level[currentLevel-1]);
        
    }

    public Transform GetPodiumPlace(){
        return level[currentLevel-1].PodiumPlace[Rank++];
    }



    
    


    


    



}
