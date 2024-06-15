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

    public int GetNumberOfPlatform(){
        return generatedMap.NumberOfPlatforms;
    }

    /// <summary>
    /// Generate new level
    /// </summary>
    public void GenerateLevel()
    {
        DestroyLevel();
        Rank=0;
        currentLevel = UserDataManager.Ins.CurrentLevel;
        generatedMap = Instantiate(levels[currentLevel - 1]);
    }


    /// <summary>
    /// Destroy current level
    /// </summary>
    public void DestroyLevel()
    {
        if (generatedMap == null)
        {
            return;
        }
        Destroy(generatedMap.gameObject);
        SimplePool.CollectAll();
        UIManager.Ins.CloseUI<IngameUI>();
    }


    /// <summary>
    /// Move camera to target position
    /// </summary>
    /// <param name="target"></param>
    public void ChangeCameraSpotlight(Transform target){
        cameraFollow.Target=target;
    }

    /// <summary>
    /// Get the position of each place 
    /// </summary>
    /// <returns></returns>
    public Transform GetPodiumPlace()
    {
        Transform pos= generatedMap.PodiumPlace[Rank];
        Rank++;
        return pos;
    }

    public void EnableLevelPlatform(int platform,int color){
        generatedMap.EnableLevelPlatform(platform,color);
    }

    public List<Brick> GetPLatformBricks(int platform){
        return generatedMap.GetPLatformBricks(platform);
    }

}
