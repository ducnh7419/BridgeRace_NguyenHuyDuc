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
    [SerializeField] private List<Level> levels;
    private Level generatedMap;
    private int currentLevel = 0;
    public int Rank = 0;

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

    public void IncreseLevel(){
        currentLevel++;
    }

    public void GenerateLevel()
    {
        DestroyLevel();
        Rank=0;
        currentLevel = GameManager.Ins.UserData.CurrentLevel;
        generatedMap = Instantiate(levels[currentLevel - 1]);    
    }



    public void DestroyLevel()
    {
        if (generatedMap == null)
        {
            return;
        }
        Destroy(generatedMap.gameObject);
        SimplePool.CollectAll();
        Platform.ClearPlatformBrickList();
        UIManager.Ins.CloseUI<IngameUI>();
    }

    public Transform GetPodiumPlace()
    {
        return levels[currentLevel - 1].PodiumPlace[Rank++];
    }

    

}
